/*

STATE

A WORLD contains a set of THINGS
A THING contains a set of properties and functions

*/

function isDef(value) { return typeof value !== 'undefined'; }

var GIDGET = {};

// Instantiates a runtime for a Thing, usually Gidget, but other Things can do things too.
GIDGET.Runtime = function(thing, world) {

	// The index of the current step
	this.pc = undefined;
	
	// The pc that executed before this
	this.lastPC = undefined;

	// Remember the world to run against.
	this.world = world;
	this.thing = thing;

	// Prepare the thing's state.
	this.resultsStack = [];
	this.scanned = [];
	this.analyzed = [];
	this.grabbed = [];
	this.focused = [];
	this.arguments = {};
	this.asked = [];
	
	// This is a list of conditionals to execute each step; event-driven behaviors.
	this.rules = [];

	// Determines what image is chosen for the thing.
	this.state = "default";
	
	this.decisions = [];

	this.getCurrentLine = function() {
	
		if(!this.isExecuting()) return undefined;
		else return this.steps[this.pc].representativeToken.line;
	
	}

	// Returns whether there's an active, executing execution.	
	this.isExecuting = function() { return this.pc < this.steps.length && (this.isExecutingGoal === true || thing.energy > 0); };
	
	this.steps = undefined;
	this.isExecutingGoal = undefined;
	
	// Returns true if the given thing is in the scope on the top of the scope stack.	
	this.isFocused = function(thing) { return this.focused.length > 0; };
	this.isFocusedOn = function(thing) { return this.focused.length > 0 && this.focused[0] === thing; };
	this.pushFocus = function(thing) { this.focused.unshift(thing); };
	this.popFocus = function() { return this.focused.shift(); };
	this.peekFocus = function() {
	
		if(this.focused.length > 0) {
			return this.focused[0]; 
		} else {
			return undefined;
		}

	};

	this.pushAsked = function(thing) { this.asked.unshift(thing); };
	this.popAsked = function() { return this.asked.shift(); };	
	this.peekAsked = function() { return this.asked.length > 0 ? this.asked[0] : undefined; }
			
	this.pushResults = function(results, query) { 
	
		results.query = query;
		this.resultsStack.unshift(results); 
		
	};
	this.popResults = function() { return this.resultsStack.shift(); };

	this.popResult = function() { 
	
		if(this.resultsStack.length > 0)
			return this.resultsStack[0].shift(); 
		else {
			console.error("Shouldn't be popping on an empty results stack.");
			return undefined;
		}

	};
	this.peekResult = function() { return this.resultsStack[0][0]; };
	this.peekResults = function() { return this.resultsStack.length > 0 ? this.resultsStack[0] : undefined; };

	// Shorthand for runtime Step_ side effects
	this.IncrementPC = function(runtime, increment) { 

		this.runtime = runtime;
		this.increment = increment;
		this.kind = 'IncrementPC';
		this.execute = function() { this.runtime.pc += this.increment; }
		
	};
	
	this.PopResults = function(runtime) { 

		this.runtime = runtime;
		this.kind = 'PopResults';
		this.execute = function() { this.runtime.popResults(); };

	};

	this.PushResults = function(runtime, results, query) { 
 
		this.runtime = runtime;
		this.kind = 'PushResults';
		this.results = results;
		this.query = query;
		this.execute = function() { this.runtime.pushResults(this.results, this.query); };

	};

	this.PopResult = function(runtime) { 

		this.runtime = runtime;
		this.kind = 'PopResult';
		this.execute = function() { this.runtime.popResult(); };

	}

	this.ClearFocus = function(runtime) {

		this.runtime = runtime;
		this.kind = 'ClearFocus';
		this.execute = function() { while(this.runtime.isFocused()) this.runtime.popFocus(); };
		
	};
	
	this.ClearResults = function(runtime) { 

		this.runtime = runtime;
		this.kind = 'ClearResults';
		this.execute = function() { 
			while(this.runtime.hasRecentResults()) this.runtime.popResults(); 
		};
		
	};

	this.Move = function(runtime, rowIncrement, columnIncrement, done) { 

		this.runtime = runtime;
		this.kind = 'Move';
		this.rowIncrement = rowIncrement;
		this.columnIncrement = columnIncrement;
		this.execute = function() { 

			this.runtime.world.place(this.runtime.thing, this.runtime.thing.row + this.rowIncrement, this.runtime.thing.column + this.columnIncrement);
			// Place all of the things this thing has grabbed.						
			var i;
			for(i = 0; i < this.runtime.grabbed.length; i++) {
				this.runtime.world.place(this.runtime.grabbed[i], this.runtime.thing.row, this.runtime.thing.column);
			}
			
		};

	};
	
	this.PushScanned = function(runtime, thing) {

		this.runtime = runtime;
		this.kind = 'PushScanned';
		this.execute = function() {
			if($.inArray(thing, runtime.scanned) < 0)
				runtime.scanned.unshift(thing);
		};

	};

	this.PushAnalyzed = function(runtime, thing) {

		this.runtime = runtime;
		this.kind = 'PushAnalyzed';
		this.execute = function() {
			if($.inArray(thing, runtime.analyzed) < 0)
				runtime.analyzed.unshift(thing);
		};

	};
	
	this.PushGrabbed = function(runtime, thing) {

		this.runtime = runtime;
		this.kind = 'PushGrabbed';
		this.thing = thing;
		this.execute = function() {
		
			// If the thing isn't grabbed, grab it.
			if($.inArray(this.thing, this.runtime.grabbed) < 0) {
			
				// Remove the thing from everything else that's grabbed it.
				var i;
				for(i = 0; i < this.runtime.world.things.length; i++) {
				
					var index = $.inArray(this.thing, this.runtime.world.things[i].runtime.grabbed);
					if(index >= 0)
						this.runtime.world.things[i].runtime.grabbed.splice(index, 1);
						
				}

				// Grab it!				
				this.runtime.grabbed.unshift(this.thing);
				
			}
			
		};

	};

	this.PopGrabbed = function(runtime, thing) {

		this.runtime = runtime;
		this.kind = 'PopGrabbed';
		this.thing = thing;
		this.execute = function() {
			// Save this for later so the UI can avoid computing it.
			this.index = $.inArray(this.thing, this.runtime.grabbed);
			runtime.grabbed.splice(this.index, 1);
		};

	};

	this.PushFocus = function(runtime, thing) {

		this.runtime = runtime;
		this.kind = 'PushFocus';
		this.thing = thing;
		this.execute = function() {
			this.runtime.pushFocus(this.thing);
		};
		
	};

	this.PopFocus = function(runtime, thing) {

		this.runtime = runtime;
		this.kind = 'PopFocus';
		this.execute = function() {
			this.runtime.popFocus();
		};

	};

	this.Say = function(runtime, message) {

		this.runtime = runtime;
		this.kind = 'Say';
		this.message = message;
		this.execute = function() {};

	};

	this.addDecision = function(step, message, action) { this.decisions.push(new this.Decision(this, step, message, action)); }
				
	this.hasRecentResults = function() { return this.resultsStack.length > 0 && this.resultsStack[0].length > 0; };

	this.queryMatches = function(query, thing) {

		// If the query is undefined, then it matches.	
		if(!isDef(query)) return true;

		var name = query.text;

		if(name.toLowerCase() === thing.name.toLowerCase()) return true;
		if(name.length > 1 && name.charAt(name.length - 1).toLowerCase() === 's' && name.substring(0, name.length - 1).toLowerCase() === thing.name.toLowerCase()) 
			return true;
	
		// Does the query match the names of one of the arguments? Is the thing passed in
		// assigned to the argument?
		if(this.arguments.hasOwnProperty(name) && this.arguments[name] === thing)
			return true;
		
		return false;
	
	};

	this.step = function(executingRule) {

		// Regardless of whether this object is done, we execute its rules.
		var i, j;
		var steps;
		
		// If we're not executing a rule, execute steps.
		if(!isDef(executingRule)) {
			for(i = 0; i < this.rules.length; i++) {
		
				// Remember the old instructions and steps.
				var ongoingSteps = this.steps;
				var ongoingPC = this.pc;
				
				this.steps = this.rules[i].steps;
				this.pc = 0;
				
				// Execute all of the steps until done.
				while(this.pc < this.steps.length) {
					
					// Execute this step.					
					var decisions = this.step(true);
					for(j = 0; j < decisions.length; j++)
						decisions[j].execute();
					
				}
					
				this.steps = ongoingSteps;
				this.pc = ongoingPC;
			
			}
			
		}
		
		// If this code is done, no need to execute further.
		if(this.pc < 0 || this.pc >= this.steps.length) {
			return undefined;
		}
		
		if (GIDGET.experiment.verbose) {
		 console.log("[" + thing.name + "]: " + this.pc + " " + this.steps[this.pc].toString());
		}

		var step = this.steps[this.pc];
		
		// Remember the last program counter for the UI, so it can know what executed.
		this.lastPC = this.pc;

		// Execute the step, gathering decisions.
		step.execute(this);
		
		// Gather the decisions made for the caller to execute individually.
		var decisions = this.decisions;
		
		// Restore the decisions to an empty array.
		this.decisions = [];

		// Ask how much energy to deduct.
		var deduction = step.cost(this);

		// Deduct the energy.
		this.thing.energy -= deduction;
		
		//ADAPTIVE: maintain energy record
		GIDGET.ui.minEnergy = GIDGET.ui.minEnergy < this.thing.energy ? GIDGET.ui.minEnergy : this.thing.energy;
		
		if(this.thing.name == "gidget"){
			GIDGET.ui.energyUsed += deduction;
		}
		console.log(GIDGET.ui.energyUsed);

		return decisions;
		
	};

	// Arguments is an object literal of things, keyed by the things' names.
	this.start = function(code, isGoal, arguments) {
	
		// Parse the program
		this.steps = isGoal === true ? GIDGET.parser.parseGoal(code) : GIDGET.parser.parseScript(code);
		this.isExecutingGoal = isGoal;

		
		// Print out the steps, just for reference
		if (GIDGET.experiment.verbose) {
			var i;
			for(i = 0; i < this.steps.length; i++) {
				console.log(i + " " + this.steps[i].toString());	
			}
		}
		
		// Prepare the execution metadata.
		this.pc = 0;
		this.resultsStack = [];
		this.focused = [];
		this.decisions = [];
		this.asked = [];
		this.arguments = arguments;
		this.isGoal = isGoal;
	
	};
	
	// Represents a decision that a Step_ made while executing. Messages can refer to
	// runtime state, which can then be highlighted in the user interface to explain the decision.
	this.Decision = function(runtime, step, message, action) {
		
		this.runtime = runtime;
		
		this.step = step;
		
		// Remember the function to execute.
		this.action = action;

		this.thought = message;

		this.execute = function() {
		
			if(isDef(action))
				action.execute();
				
			if(isDef(this.thought.emotion))
				this.runtime.state = this.thought.emotion;
		
		};
			
	};
	
};

// The collection of steps that know how to execute a Gidget program, produced by the parser.
GIDGET.runtime = {
	
	Step_IF: function(ast, representativeToken, offset) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			offset: offset,
			toString: function() { return "if " + this.offset; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {		

				var results = runtime.peekResults();

				if(results.length > 0) {
				
					runtime.addDecision(
						this,
						GIDGET.text.if_true(),
						new runtime.IncrementPC(runtime, 1));

				}
				else {

					runtime.addDecision(
						this,
						GIDGET.text.if_false(),
						new runtime.IncrementPC(runtime, this.offset));
					
				}

				runtime.addDecision(
					this,
					GIDGET.text.if_popResults(),
					new runtime.PopResults(runtime));

			}
		};
	},

	Step_WHEN: function(ast, representativeToken, steps) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			steps: steps,
			toString: function() { return "when"; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {		
			
				// Add the rule to the thing's runtime's cognizant list of rules.
				runtime.rules.push(this);
				
				// Next step
				runtime.pc++;
			
			}
		};			
	},

	Step_IS: function(ast, representativeToken, keyword, name) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			keyword: keyword,
			name: name,
			toString: function() { return "" + this.keyword.text + " " + this.name.text; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {		
				
				var results = runtime.peekResults();
				var newResults = [];
				var i;
				var hasTag = undefined;
				var positive = keyword.text === 'is' || keyword.text === 'are';
				
				var allTrue = true;
				
				for(i = 0; i < results.length; i++) {
			
					hasTag = results[i].tags.hasOwnProperty(this.name.text.toLowerCase());

					if((positive && hasTag) || (!positive && !hasTag)) {
					
						runtime.addDecision(this, GIDGET.text.is_positive(results[i].name, i, this.keyword.text, this.name.text));
							
						newResults.push(results[i]);
						
					}
					else {
						allTrue = false;
						runtime.addDecision(this, GIDGET.text.is_negative(results[i].name, i, this.keyword.text, this.name.text));
						
					}
				
				}
				
				runtime.addDecision(
					this,
					GIDGET.text.is_popResults(), 
					new runtime.PopResults(runtime));

				runtime.addDecision(
					this,
					GIDGET.text.is_newResults(), 
					new runtime.PushResults(runtime, allTrue ? newResults : []));

				runtime.pc++;
		
			}
		};
	},
			
	Step_UNKNOWN: function(ast, representativeToken, tokens, message) {
	
		return {
			ast: ast,
			representativeToken: representativeToken,
			tokens: tokens,
			message: message,
			toString: function() { 
			
				var i;
				var array = "[";
				for(i = 0; i < this.tokens.length; i++)
					array = array + this.tokens[i].text + (i < this.tokens.length - 1 ? "," : "");
			
				return 'unknown ' + array + ']'; 
				
			},
			cost: function(runtime) { return 0; },
			execute: function(runtime) {

				var i;
				var string = "";
				for(i = 0; i < this.tokens.length; i++)
					string = string + this.tokens[i].text + " ";

				runtime.addDecision(
					this,
					this.message,
					undefined);

				// Clear the results and focus stacks
				runtime.addDecision(
					this,
					GIDGET.text.unknown_clearFocus(), 
					new runtime.ClearFocus(runtime));

				runtime.addDecision(
					this,
					GIDGET.text.unknown_clearResults(), 
					new runtime.ClearResults(runtime));

				runtime.addDecision(
					this,
					GIDGET.text.unknown_nextStep(),
					new runtime.IncrementPC(runtime, 1));
				
			}
		};
	},
	
	// Scans the next thing on the list.
	Step_SCAN: function(ast, representativeToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			offset: undefined,
			toString: function() { return "scan " + this.offset; },
			cost: function(runtime) { return 1; },
			execute: function(runtime) {

				if(runtime.hasRecentResults()) {
				
					runtime.addDecision(
						this,
						GIDGET.text.scan_success(runtime.peekResult().name),
						new runtime.PushScanned(runtime, runtime.peekResult()));
						
					runtime.pc++;
					
				}
				else {

					runtime.pc += this.offset;
				
				}
					
			}
		};
	},

	// Scans the next thing on the list.
	Step_NAME: function(ast, representativeToken, name) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			name: name,
			offset: undefined,
			toString: function() { return "name " + this.name.text; },
			cost: function(runtime) { return 1; },
			execute: function(runtime) {

				if(runtime.hasRecentResults()) {
				
					var result = runtime.peekResult();
					
					result.name = this.name.text;
					runtime.addDecision(this, GIDGET.text.name_success(this.name.text));

					runtime.pc++;
									
				}
				else {

					runtime.pc += this.offset;
				
				}

			}
		};
	},

	// Go to the next object in the query on the stack
	Step_GO: function(ast, representativeToken, avoidToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			avoid: avoidToken,
			offset: undefined,
			toString: function() { return "go " + (isDef(this.avoid) ? "avoid " + this.avoid.text + " " : "") + this.offset; },
			cost: function(runtime) { 
			
				return 1 + runtime.grabbed.length; 
					
			},
			execute: function(runtime) {
			
				// If we haven't set the clock, set it.
				if(!isDef(this.wait)) {
					this.wait = runtime.thing.speed;				
				}
				
				// Count down the wait.
				this.wait--;
				
				// If it's still above zero, skip moving.
				if(this.wait > 0)
					return;
					
				// Otherwise, reset the clock and move!
				this.wait = runtime.thing.speed;				
			
				// Are there any targets to move to?
				if(runtime.hasRecentResults()) {

					// What are we going to?
					var thing = runtime.peekResult();

					// Dijkstra's algorithm, adapted to find shortest path from Gidget to target.
					function findShortestPath(grid, currentRow, currentColumn, goalRow, goalColumn, level, nameToAvoid, checkWithinOne) {

						function isValid(queue, cells, grid, row, column, level, theNameToAvoid, withinOne) {
						
							// If it's outside the bounds of the world, it's not a good location.
							if(row < 0) return false;
							if(row >= grid.length) return false;
							if(column < 0) return false;
							if(column >= grid[0].length) return false;

							// Check height restrictions at this location.
							var i;
							for(i = 0; i < grid[row][column].length; i++) {
								var thing = grid[row][column][i]; 
								if(thing.level > level) {
									return false;
								}
							}

							// Have we already checked this? Not valid.							
							if($.inArray(cells[row][column], queue) < 0) return false;

							// Check for things to avoid.
							if(isDef(theNameToAvoid)) {
								
								// Are we checking within one?
								if(withinOne) {
									var nearby = [[row-1, column],[row-1,column-1],[row,column-1],[row+1,column-1],[row+1,column],[row+1,column+1],[row,column+1],[row-1,column+1]];
									var spot;
									for(spot = 0; spot < nearby.length; spot++) {
										var r = nearby[spot][0]
										var c = nearby[spot][1];
										// If this is a legal position
										if(r >= 0 && r < grid.length && c >= 0 && c < grid[0].length) {
											// Go through all of the things at this position...
											for(i = 0; i < grid[r][c].length; i++) {
												// Does the thing have the same name as the thing to avoid?
												var thing = grid[r][c][i]; 
												if(isDef(thing.name) && thing.name.toLowerCase() === theNameToAvoid.toLowerCase()) {
													return false;
												}
											}
										}
									}
								}
								// Otherwise, just check for the cell itself
								else {
									for(i = 0; i < grid[row][column].length; i++) {
										// Does the thing have the same name as the thing to avoid?
										var thing = grid[row][column][i]; 
										if(isDef(thing.name) && thing.name.toLowerCase() === theNameToAvoid.toLowerCase()) {
											return false;
										}
									}
								}
								
							}

							// Otherwise, it's valid.							
							return true; 
						
						}

						// Make a queue of cells.
						var queue = [];

						// Make a grid of cells for bookkeeping.						
						var cells = new Array(grid.length);
						var row, col;
						for(row = 0; row < grid.length; row++) {
							cells[row] = new Array(grid[0].length);
							for(col = 0; col < grid[0].length; col++) {
								cells[row][col] = { 
									row: row, 
									column: col, 
									distance: undefined, 
									previous: undefined
								};
								// Add all cells to the queue.
								queue.push(cells[row][col]);
							}
						}
						
						var start = cells[currentRow][currentColumn];
						var goal = cells[goalRow][goalColumn];
						start.distance = 0;
						
						while(queue.length > 0) {
						
							// Find the cell with the smallest distance.
							var i;
							var closestCell = undefined;
							for(i = 0; i < queue.length; i++) {
								if(isDef(queue[i].distance) && (closestCell === undefined || queue[i].distance < closestCell.distance))
									closestCell = queue[i];
							}
							
							// If no cell remains that is accessible from source, we have failed.
							if(closestCell === undefined)
								break;

							// Remove the closest cell from the queue.	
							queue.splice($.inArray(closestCell, queue), 1);
						
							// Gather a list of valid neighbors of the closest cell.
							var neighbors = [];
							
							// Is the cell above a valid, passable, that has not been visited?
							if(isValid(queue, cells, grid, closestCell.row - 1, closestCell.column, level, nameToAvoid, checkWithinOne))
								neighbors.push(cells[closestCell.row - 1][closestCell.column]);
							if(isValid(queue, cells, grid, closestCell.row + 1, closestCell.column, level, nameToAvoid, checkWithinOne))
								neighbors.push(cells[closestCell.row + 1][closestCell.column]);
							if(isValid(queue, cells, grid, closestCell.row, closestCell.column - 1, level, nameToAvoid, checkWithinOne))
								neighbors.push(cells[closestCell.row][closestCell.column - 1]);
							if(isValid(queue, cells, grid, closestCell.row, closestCell.column + 1, level, nameToAvoid, checkWithinOne))
								neighbors.push(cells[closestCell.row][closestCell.column + 1]);
							
							// For each valid neighbor, 
							for(i = 0; i < neighbors.length; i++) {
							
								var neighbor = neighbors[i];
								var alt = closestCell.distance + 1;
								// If navigating to this neighbor is less than the distance of the neighbor to the goal,
								// then update the neighbors distance.
								if(neighbor.distance === undefined || alt < neighbor.distance) {
							
									neighbor.distance = alt;
									neighbor.previous = closestCell;
							
								}
							
							}
						
						}
						
						var path = [];
						while(isDef(goal)) {

							path.unshift(goal);
							goal = goal.previous;
						
						}		
						
						// If the path is non empty and begins at the start point, then return it.
						if(path.length > 0 && path[0].row === currentRow && path[0].column === currentColumn)
							return path;
						// Otherwise, an empty array represents failure.
						else
							return [];
					
					}	

					var avoidText = isDef(this.avoid) ? this.avoid.text : undefined;

					// Find the shortest path to the target using the above algorithm, avoid thing things within 1 unit
					var path = findShortestPath(runtime.world.grid, runtime.thing.row, runtime.thing.column, thing.row, thing.column, runtime.thing.level, avoidText, true);

					// If we were avoiding and there was no path, try just avoiding going on top of things.
					var noPath = false;
					if(isDef(this.avoid) && path.length <= 1) {
						noPath = true;
						path = findShortestPath(runtime.world.grid, runtime.thing.row, runtime.thing.column, thing.row, thing.column, runtime.thing.level, avoidText, false);
					}
					
					// If we're avoiding and there was still no path, try not avoiding at all.
					if(isDef(this.avoid) && path.length <= 1) {
						noPath = true;
						path = findShortestPath(runtime.world.grid, runtime.thing.row, runtime.thing.column, thing.row, thing.column, runtime.thing.level, undefined, false);
					}

					// If there was a path, remember it.
					if(path.length > 1) {
											
						// Move the next direction in the path
						var rowIncrement = path[1].row - runtime.thing.row;
						var columnIncrement = path[1].column - runtime.thing.column;

						var done = runtime.thing.row + rowIncrement === thing.row && runtime.thing.column + columnIncrement === thing.column;

						// If we're moving somewhere, say so!
						if(rowIncrement !== 0 || columnIncrement !== 0)
							runtime.addDecision(
								this,
								noPath ? 
									GIDGET.text.go_dangerousStep(thing.name, this.avoid.text) :
									GIDGET.text.go_step(thing.name),
								new runtime.Move(runtime, rowIncrement, columnIncrement, done));

						// We're at it! Execute the command on it, if there is one, otherwise, go to the next thing.
						if(done) {
	
							runtime.path = undefined;
							runtime.addDecision(
								this,
								GIDGET.text.go_arrive(thing.name),
								new runtime.IncrementPC(runtime, 1));
						
						}
					
					}
					else if(runtime.thing.row === thing.row && runtime.thing.column === thing.column) {
					
						runtime.addDecision(
							this,
							GIDGET.text.go_alreadyAt(thing.name),
							new runtime.IncrementPC(runtime, 1));
					
					}
					// There are two behavior's we've had implemented in the past here: one is to keep
					// looping until a path opens up, the other is to just continue forward.
					else {
					
						runtime.path = undefined;
						runtime.addDecision(
							this,
							GIDGET.text.go_noPath(thing.name),
							new runtime.IncrementPC(runtime, 1));
					
					}
				
				}
				else {

					runtime.addDecision(
						this,
						GIDGET.text.go_finished(),
						new runtime.IncrementPC(runtime, this.offset));

				}
								
			}
		};
	},

	// Scans the next thing on the list.
	Step_ANALYZE: function(ast, representativeToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			offset: undefined,
			toString: function() { return "analyze " + this.offset; },
			cost: function(runtime) { return 1; },
			execute: function(runtime) {

				if(runtime.hasRecentResults()) {
				
					runtime.addDecision(
						this,
						GIDGET.text.analyze_success(runtime.peekResult().name),
						new runtime.PushAnalyzed(runtime, runtime.peekResult()));
						
					runtime.pc++;
									
				}
				else {

					runtime.pc += this.offset;
				
				}
					
			}
		};
	},

	// Pops the next Thing in the query result on the top of the stack.
	// If there's nothing left in the query, go to the next instruction.
	Step_NEXT: function(ast, representativeToken, offset) {

		return {
			ast: ast,
			representativeToken: representativeToken,
			offset: offset,
			toString: function() { return "next " + this.offset; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {

				// Assuming there's a result left, pop it since we're done with it.
				if(runtime.hasRecentResults())
					runtime.popResult();

				// If there are results left, continue to the next one.
				if(runtime.hasRecentResults()) {
											
					runtime.pc += this.offset;
					
				}
				// If there aren't results left, pop the empty list and move on to the next step.
				else {

					runtime.popResults();

					runtime.pc++;
					
				}

			}
		};
		
	},
	
	Step_GRAB: function(ast, representativeToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			offset: undefined,
			toString: function() { return "grab"; },
			cost: function(runtime) { return 1; },
			execute: function(runtime) {

				if(runtime.hasRecentResults()) {
					runtime.addDecision(
						this,
						GIDGET.text.grab_success(runtime.peekResult().name),
						new runtime.PushGrabbed(runtime, runtime.peekResult()));
						
					runtime.pc++;
					
				}
				else {
					runtime.pc+= this.offset;
				}
					
			}
		};
	},
	
	Step_DROP: function(ast, representativeToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			offset: undefined,
			toString: function() { return "drop"; },
			cost: function(runtime) { return 1; },
			execute: function(runtime) {

				if(runtime.hasRecentResults()) {

					runtime.addDecision(
						this,
						GIDGET.text.drop_success(runtime.peekResult().name),
						new runtime.PopGrabbed(runtime, runtime.peekResult()));		

					runtime.pc++;
						
				}
				else {
					runtime.pc += this.offset;
				}
				
						
			}
		};
	},
	
	Step_MODIFY: function(ast, representativeToken, keyword, property, number) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			keyword: keyword,
			property: property,
			number: number,
			toString: function() { return "" + this.keyword.text + " " + property.text + " " + (isDef(this.number) ? this.number : ""); },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {

				var result;
				
				if(runtime.hasRecentResults()) {

					var amount = isDef(this.number) ? this.number : 1;

					result = runtime.peekResult();					

					if(this.keyword.text === 'raise') {
						if(this.property.text === 'height') result.level += amount;
						else if(this.property.text === 'energy') result.energy += amount;
					}
					else if(this.keyword.text === 'lower') {
						if(this.property.text === 'height') result.level -= amount;
						else if(this.property.text === 'energy') result.energy -= amount;
					}
					else if(this.keyword.text === 'set') {
						if(this.property.text === 'height') result.level = amount;
						else if(this.property.text === 'energy') result.energy = amount;
					}

					runtime.addDecision(this, GIDGET.text.modify(this.keyword.text, this.property.text, isDef(this.number) ? this.number.text : ""));

					runtime.pc++;

				}
				else {
					runtime.pc+= this.offset;
				}
					
			}
		};
	},

	Step_ADD: function(ast, representativeToken, name) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			name: name,
			toString: function() { return "add " + this.name.text; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {

				var thing = new GIDGET.Thing(runtime.world, name.text, runtime.thing.row, runtime.thing.column, "rgb(250,255,255)", ["white"], {});

				thing.runtime.start(thing.code, false, {});

				runtime.addDecision(this, new GIDGET.text.Message("I created " + thing.name + "."));

				runtime.pc++;

			}
		};
	},

	Step_REMOVE: function(ast, representativeToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			toString: function() { return "remove"; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {

				if(runtime.hasRecentResults()) {

					var thing = runtime.peekResult();

					runtime.world.removeThing(thing);

					runtime.addDecision(this, new GIDGET.text.Message("I removed " + thing.name + "."));
	
					runtime.pc++;
					
				}
				// If there are no results, skip the focus block.
				else {
				
					runtime.pc+= this.offset;

				}

			}
		};
	},

	Step_QUERY: function(ast, parentAST, representativeToken, name, constraints, scoped) {
		return {
			ast: ast,
			parentAST: parentAST,
			representativeToken: representativeToken,
			name: name,
			constraints: constraints,
			scoped : scoped,
			toString: function() { 
			
				var cons = "";
				var i;
				for(i = 0; i < constraints.length; i++)
					cons = cons + constraints[i] + " ";
			
				return 'query ' + (scoped ? "on " : "") + "" + (isDef(this.name) ? this.name.text : '') + ' ' + cons; 				
				
			},
			cost: function(runtime) { return 0; },
			execute: function(runtime) {

				var scope = [];
				var i;
				var temp;

				// If the results are scoped, get the scope from the stack.
				if(this.scoped === true) {

					var scopeResults = runtime.popResults();
					
					// If there's some Thing on the results, make the scope all of the objects that are at the same level and
					// location as the thing.
					if(scopeResults.length > 0) {

						var scopeThing = scopeResults[0];
						for(i = 0; i < runtime.world.things.length; i++) {
						
							var worldThing = runtime.world.things[i];
							if(	scopeThing.level === worldThing.level && 
								scopeThing.row === worldThing.row && 
								scopeThing.column === worldThing.column)
								scope.push(worldThing);						
						
						}
					
					}
					// If there was nothing, then the query scope is nothing.
					else {
						scope = []
					}
					
				}
				else {				

					// The scope includes everything. Don't use the thing array though, create a new one.
					scope = scope.concat(runtime.world.things);
					
				}

				function filter(list, keep) {
				
					var keepers = [];
					var i;
					
					// Keep all things that are at the level of this object.
					for(i = 0; i < list.length; i++) {
						if(keep.call(undefined, list[i])) {
							keepers.push(list[i]);
						}
					}
					return keepers;
				
				}

				// Now, narrow down the results according to the supplied constraints.
				// nearest | first | second | third | last | grabbed | reachable | scanned | analyzed | level | over | focused
				if($.inArray("focused", this.constraints) >= 0) {
				
					scope = filter(scope, function(thing) { 
						return thing === runtime.peekFocus(); });
				
				}
				
				if($.inArray("level", this.constraints) >= 0) {
				
					scope = filter(scope, function(thing) { 
						return thing.level == runtime.thing.level; });
				
				}
				
				if($.inArray("scanned", this.constraints) >= 0) {

					scope = filter(scope, function(thing) { 
						// @DEPRECATED The code commented out below originally allowed things to go to themselves without having scanned themselves.
						// This isn't necessary though, and breaks the first gidget level, where he scans himself to know where he is.
						// 
						// 3/8/2011 We've decided that things *should* know themselves after all.
						return thing === runtime.thing || $.inArray(thing, runtime.scanned) >= 0; });
				
				}

				if($.inArray("analyzed", this.constraints) >= 0) {

					scope = filter(scope, function(thing) { 
						return thing === runtime.thing || $.inArray(thing, runtime.analyzed) >= 0; });
				
				}

				if($.inArray("grabbed", this.constraints) >= 0) {
				
					scope = filter(scope, function(thing) { 
						return $.inArray(thing, runtime.grabbed) >= 0; });
				
				}

				if($.inArray("over", this.constraints) >= 0) {

					scope = filter(scope, function(thing) { 
						return thing.row === runtime.thing.row && thing.column === runtime.thing.column; });
				
				}
				
				if($.inArray("reachable", this.constraints) >= 0) {
				
					// TODO				
				
				}

				var nameToMatch = this.name;

				// Now filter by names.
				scope = filter(scope, function(thing) { 
				
					if(nameToMatch.text.toLowerCase() === 'it') return thing === runtime.peekFocus();
					else return runtime.queryMatches(nameToMatch, thing); 
				
				});


				// After matching by names, do size and index filters.
				if($.inArray("one", this.constraints) >= 0) {
				
					// Scope must be size one!
					if(scope.length !== 1) 
						scope = [];
				
				}

				if($.inArray("two", this.constraints) >= 0) {
				
					// Scope must be size one!
					if(scope.length !== 2) 
						scope = [];
				
				}

				if($.inArray("three", this.constraints) >= 0) {
				
					// Scope must be size one!
					if(scope.length !== 3) 
						scope = [];
				
				}

				if($.inArray("first", this.constraints) >= 0) {
				
					if(scope.length > 0) 
						scope = [ scope[0] ];
					else
						scope = [];
				
				}

				if($.inArray("second", this.constraints) >= 0) {
				
					if(scope.length > 1) 
						scope = [ scope[1] ];
					else
						scope = [];
				
				}

				if($.inArray("third", this.constraints) >= 0) {
				
					if(scope.length > 2) 
						scope = [ scope[2] ];
					else
						scope = [];
				
				}
				
				var description = GIDGET.text.query(this.parentAST, nameToMatch.text, scope);
				
				if(scope.length > 0) {
					
					runtime.addDecision(
						this,
						description,
						new runtime.PushResults(runtime, scope, this));

				}
				else {
					
					runtime.addDecision(
						this,
						description,
						new runtime.PushResults(runtime, scope));
						
				}
				
				// Next instruction
				runtime.pc++;
									
			}
		};
	},
	
	// Scopes future queries to the first Thing in the most recent query results.
	// Used to execute ,'ed commands on a particular thing.
	Step_SCOPE: function(ast, representativeToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			toString: function() { return "scope"; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {
		
				if(runtime.hasRecentResults()) {

					runtime.addDecision(
						this,
						GIDGET.text.focus_success(runtime.peekResult().name),
						new runtime.PushFocus(runtime, runtime.peekResult()));

				}
				else {

					runtime.addDecision(this, GIDGET.text.focus_failure(), undefined);
					
				}
	
				runtime.pc++;
					
			}
		};
	},
	
	Step_UNSCOPE: function(ast, representativeToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			toString: function() { return "unscope"; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {
	
				runtime.addDecision(
					this,
					GIDGET.text.unfocus_success(),
					new runtime.PopFocus(runtime));
	
				runtime.pc++;
								
			}
		};
	},

	Step_SAY: function(ast, representativeToken, message) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			message: message,
			toString: function() { return "say " + this.message; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {
	
				runtime.addDecision(
					this,
					message,
					new runtime.Say(runtime, message));
	
				runtime.pc++;
								
			}
		};
	},

	// Pops the two sets of results; if both are non-empty sets, pushes the intersection; if at least one is empty, pushes the empty set.
	Step_AND: function(ast, representativeToken) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			toString: function() { return "and"; },
			cost: function(runtime) { return 0; },
			execute: function(runtime) {

				var results1 = runtime.popResults();
				var results2 = runtime.popResults();
				
				if(results1.length === 0 || results2.length === 0)
					runtime.pushResults([]);
				else
					runtime.pushResults(results1.concat(results2));
					
				runtime.pc++;
								
			}
		};
	},
	
	Step_ASK: function(ast, representativeToken, query, action, arguments) {
		return {
			ast: ast,
			representativeToken: representativeToken,
			query: query,
			action: action,
			arguments: arguments,
			offset: undefined,
			toString: function() { return "ask " + (isDef(this.action) ? this.action.text : "unspecified") + " offset " + this.offset; },
			cost: function(runtime) { return isDef(runtime.peekAsked()) ? 0 : 1; },
			execute: function(runtime) {

				var arguments = [];

				// If there are no results, skip over the focus.
				if(!runtime.hasRecentResults()) {
				
					runtime.pc += this.offset;
					return;
				
				}

				// If we already asked something, then the world will run it. We just check if it's done
				// before Gidget proceeds.
				if(isDef(runtime.peekAsked())) {

					// If it's done, Gidget waits.				
					if(runtime.peekAsked().runtime.isExecuting()) {
					
						runtime.addDecision(this, GIDGET.text.ask_waiting(runtime.peekAsked().name, this.action.text));
					
					}
					// If it's done, pop the object and continue.
					else {

						runtime.addDecision(this, GIDGET.text.ask_finished(runtime.peekAsked().name));
						runtime.popAsked();
						runtime.pc++;
					
					}
				
				}
				else {
					if(runtime.resultsStack.length >= this.arguments.length) {
	
						var i, result;
						for(i = 0; i < this.arguments.length; i++) {
	
							// Resolve the argument query; the only arguments that can be passed are either the thing
							// doing the telling or other objects that have been scanned.
							var argumentOptions = runtime.popResults();

							// Take the first result in the list.							
							if(argumentOptions.length > 0) {
								arguments.unshift(argumentOptions[0]);
							}
							// Otherwise, no matches for this argument.
							else {
								arguments.unshift(undefined);
							}
	
						}
	
						var i = 0;
						var object = runtime.peekResult();
						script = undefined;

						// If we didn't find an object, we're in trouble.
						if(!isDef(object)) {

							runtime.addDecision(this, GIDGET.text.ask_noObject());

							runtime.pc += this.offset;
							return;						
						
						}

						// The action found to execute.
						var action = undefined;
						
						var objectAction = undefined;
						for(objectAction in object.actions) {
							if(object.actions.hasOwnProperty(objectAction)) {
							
								if(objectAction.toLowerCase() === this.action.text.toLowerCase()) {
									action = object.actions[objectAction];
									break;
								}
							}
						}
						

						
						// If we found the action...
						if(isDef(action)) {

							script = action.script;
							argumentNames = action.arguments;
	
							// If the user hasn't supplied the expected number of arguments, then fail
							if(argumentNames.length !== arguments.length) {

								runtime.addDecision(
									this,
									GIDGET.text.ask_missingArguments(object.name, this.action.text, argumentNames.length, arguments.length));
								
								runtime.pc += this.offset;
								return;
							
							}
							else {
	
								runtime.addDecision(this, GIDGET.text.ask_begin(object.name, this.action.text));
								
								var actualArguments = {};
		
								var argIndex = 0;
								for(argIndex = 0; argIndex < argumentNames.length; argIndex++) {								
								
									// Map given argument to argument name
									actualArguments[argumentNames[argIndex]] = arguments[argIndex];
								
								}
		
								// Start the object on the script we found.
								object.runtime.start(script, false, actualArguments);
								
								// Push it on the asked list.
								runtime.asked.push(object);
	
								// Don't increment; keep executing this step its done.
								
							}
		
						}
						else {
						
							runtime.addDecision(this, GIDGET.text.ask_unknownAction(object.name, this.action.text));
							runtime.pc += this.offset;
						}
												
					}
					else {
					
						console.error("This should never happen; there should be exactly as many arguments on the results stack as there were given.");
						runtime.pc += this.offset;
		
					}
					
				}
								
			}
		};
	}

};
