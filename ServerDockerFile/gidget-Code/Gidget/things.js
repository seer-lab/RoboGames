// Constructor for a thing, takes a name and a set of additional functions from the parser.
GIDGET.Thing = function(world, name, row, col, color, tags, actions) {

	this.name = name;		// The name by which the thing can be referred
	this.row = row;			// The row of the thing
	this.column = col;		// The column of the thing
	this.energy = 1000;		// The current energy of the thing; things with energy < 0 are "dead"
	this.level = 1;			// The current level of the object above the grid, for creating boundaries
	this.labeled = true;	// Determines whether the label of the thing is shown
	this.speed = 1;
	this.color = color;
	this.tags = {};
	this.code = "";
	
	this.setEnergy = function(energy) { this.energy = energy; };
	this.setLevel = function(level) { this.level = level; };
	this.setCode = function(code) { this.code = code; }
	this.setSpeed = function(speed) { this.speed = speed; }
	this.setLabeled = function(labeled) { this.labeled = labeled; }
	
	// Translate the list of tags into a set (well, an object literal hash table of tag keys with true values).
	var i;
	for(i = 0; i < tags.length; i++)
		this.tags[tags[i].toLowerCase()] = true;
	
	this.actions = actions;

	this.draw = function(ctx, size) {

		ctx.save();

		var padding = world.grndBorder;
		
		// Draw shadow
/*
		if(this.level > 1) {
		
			var offset = (this.level - 1) * 10;
			ctx.fillStyle = "rgba(0,0,0,.2)";
			ctx.fillRect(this.column * size + 1 + offset + padding, this.row * size + 1 + offset + padding, size - padding * 2, size - padding * 2);
		
		}		
*/
		var animateRowOffset = 0, animateColumnOffset = 0;
		
		var image= GIDGET.ui.getImage(this.name, this.runtime.state);
		if(!isDef(image)) image = GIDGET.ui.getImage('unknown', 'default');
		
		
		if(GIDGET.ui.percentRemaining > 0) {
		
			if(this.rowDelta !== 0) animateRowOffset = -(GIDGET.ui.percentRemaining / 100.0) * size * this.rowDelta;
			if(this.columnDelta !== 0) animateColumnOffset = -(GIDGET.ui.percentRemaining / 100.0) * size * this.columnDelta;
		
		}

		// Compute a level offset, so that things that are a certain height go above their cell.
		var levelOffset = this.level > 1 ? (this.level - 1) * size : 0;

		if(isDef(image) && image.width > 0 && image.height > 0 && size > 0) {
			ctx.drawImage(image, this.column * size + padding + animateColumnOffset, this.row * size + padding + animateRowOffset - levelOffset, size - padding * 2, size - padding * 2 + levelOffset);
		}
		else {
				
			ctx.fillStyle = this.color;		
			ctx.fillRect(this.column * size + padding + animateColumnOffset, this.row * size + padding + animateRowOffset - levelOffset, size - padding * 2, size - padding * 2 + levelOffset);
		}
		
		ctx.restore();
	
	}

	world.addThing(this);		
	
	// Give the thing a runtime.
	this.runtime = new GIDGET.Runtime(this, world);

}

// *******************************************************
// *** A C T I O N - C O N S T R U C T O R ***************
// *******************************************************


GIDGET.Action = function(arguments, script) {

	return {
		arguments: arguments,
		script: script
	};

};

// *******************************************************
// *** W O R L D - C O N S T R U C T O R *****************
// *******************************************************


GIDGET.World = function(gridSize, gidgetPos, groundAtt, code) {
	
	// Grid (World) size & Gidget's initial positioning + starting energy
	var rowCount = isDef(gridSize[0]) ? gridSize[0] : 10;
	var colCount = isDef(gridSize[1]) ? gridSize[1] : rowCount;
	var gidgetRow = gidgetPos[0];
	var gidgetCol = gidgetPos[1];
	var gidgetEnergy = isDef(gidgetPos[2]) ? gidgetPos[2] : 100;
	
	//Adaptive Properties
	var adaptEnergy; //Minimum energy required
	this.lowAdapt = function(){}; //low competence adaptation
	this.highAdapt = function(){}; //high competence adaptation
	
	// Ground Attributed: image/name, color, and border width
	this.grnd = isDef(groundAtt[0]) ? groundAtt[0] : "dirt";
	this.grndColor = isDef(groundAtt[1]) ? groundAtt[1] : "rgb(124,57,10)";
	this.grndBorder = isDef(groundAtt[2]) ? groundAtt[2] : 2;

	// Set level name
	this.levelTitle = "";
	this.levelNumber;
	this.numberOfLevels;
	
	// Remember the initial code
	this.code = code;

	// DEPRECATED - Remember the mission so it can be placed in the UI.	
	//this.mission = isDef(mission) ? mission : "I don't know what I'm supposed to do here. No one gave me a mission :(";
	
	this.missionText = [];

	// Remember the goal
	this.goals = [];

	// All of the things in the world, starting with a Gidget
	this.things = [];

	// An array of arrays of arrays, representing a 2D grid where each cell contains a list of references to things.
	// This creates an empty grid.
	this.grid = new Array(rowCount);
	var row, col;
	for(row = 0; row < rowCount; row++) {
		this.grid[row] = new Array(colCount);
		for(col = 0; col < colCount; col++) {
			this.grid[row][col] = [];
		}
	}

	this.addTitle = function (title) {
		this.levelTitle = isDef(title) ? title : "";
		this.addLevelNumber();	// Also updates level number
	}

	this.addLevelNumber = function() {
		// Get level number
		var lvlNumber,index = 1;
		for(var level in GIDGET.levels) {	
			if(GIDGET.levels.hasOwnProperty(level) && localStorage.currentLevel === level) {
				this.levelNumber = index;
			}
			index++;
		}
		this.numberOfLevels = index-1;
	}

	this.hiddenCommands = [];

	this.addHiddenCommand = function(name) {
	
		this.hiddenCommands[name] = true;
	
	};
 
	// Add a paragraph of text and an associated emotional state for it.
	this.addMissionText = function(state, text) {

		this.missionText.push({ text: text, state: state });
	
	};

	this.addGoal = function(goal) {
	
		this.goals.push(goal);
	
	};

	this.addThing = function(thing) {
		
		this.things.push(thing);
		this.place(thing, thing.row, thing.column);
	
	};
	
	this.removeThing = function(thing) {
	
		var index = $.inArray(thing, this.things);
		if(index >= 0) {
		
			// Remove from thing list
			this.things.splice(index, 1);
			
			// Remove from grid
			index = $.inArray(thing, this.grid[thing.row][thing.column]);
			if(index >= 0) {
				this.grid[thing.row][thing.column].splice(index, 1);
			}
			else {
				console.error("We shouldn't be able to remove thing from thing list but not grid");
			}

			// Remove from scanned
			index = $.inArray(thing, this.gidget.runtime.scanned);
			if(index >= 0) this.gidget.runtime.scanned.splice(index, 1);

			// Remove from analyzed
			index = $.inArray(thing, this.gidget.runtime.analyzed);
			if(index >= 0) this.gidget.runtime.analyzed.splice(index, 1);

			// Remove from grabbed
			index = $.inArray(thing, this.gidget.runtime.grabbed);
			if(index >= 0) this.gidget.runtime.grabbed.splice(index, 1);
			
		}
		else {
			console.error("It shouldn't be possible to remove something that isn't in this world.");
		}
		
	};

	this.resetThingDeltas = function() {
	
		for(var i = 0; i < this.things.length; i++) {
			this.things[i].rowDelta = 0;
			this.things[i].columnDelta = 0;
		}
	
	};

	// First, define how to place a thing.
	this.place = function(thing, row, col) {
	
		if(!this.isLegalColumn(row, col)) {
			console.error("" + row + " " + col + " isn't legal");
			return;
		}

		if(this.isLegalColumn(thing.row, thing.column)) {
				
			var index = this.grid[thing.row][thing.column].indexOf(thing);
			if(index >= 0)
				this.grid[thing.row][thing.column].splice(index, 1);
				
		}

		// Remember the most recent change.
		thing.rowDelta = row - thing.row;
		thing.columnDelta = col - thing.column;

		thing.row = row;
		thing.column = col;

		this.grid[thing.row][thing.column].push(thing);
	
	};
	
	// Define how to check legal positions.
	this.isLegalRow = function(row) { return row !== undefined && row >= 0 && row < this.grid.length; };
	this.isLegalColumn = function(row, col) { return this.isLegalRow(row) && col !== undefined && col >= 0 && col < this.grid[row].length; };

	// Add a bunch of ground things.
	for(row = 0; row < rowCount; row++) {
		for(col = 0; col < colCount; col++) {
			
			var ground = new GIDGET.Thing(this, this.grnd, row, col, this.grndColor, {}, {}); // Brown: rgb(164,77,30)
			ground.setLevel(0);
			ground.setLabeled(false);
			
		}
	}

	// Add gidget. Every world has a gidget in it.
	this.gidget = new GIDGET.Thing(this, "gidget", gidgetRow, gidgetCol, "rgb(50,50,50)", {}, {});
	this.gidget.setEnergy(gidgetEnergy);
	
	// This is the function that causes each thing in the world to step one step, in order.
	// It takes the code passed to the world and assigns it to Gidget.
	this.start = function(gidgetScript) {
	
		// Prepare all of the other Things in the world for execution.
		var i;
		for(i = 0; i < this.things.length; i++) {
		
			if(this.things[i] !== this.gidget)
				this.things[i].runtime.start(this.things[i].code, false, {});
		
		}
	
		// This prepares gidget for execution.
		this.gidget.runtime.start(gidgetScript, false, {});
	
	};
	
	// The world is executing while Gidget has steps that he's executing. The runtime is
	// executing while a thing is executing the goal or is not executing a goal and still has energy.
	this.isExecuting = function() {
	
		return isDef(this.gidget) && isDef(this.gidget.runtime) && isDef(this.gidget.runtime.steps) && this.gidget.runtime.isExecuting();

	};
	
	// Steps all objects in the world one step, gathering all of their decisions into a list of lists.
	// that the caller can use to execute each decision one at a time.	
	this.step = function() {

		var decisions = [];
	
		// Step each thing in the world once, executing the resulting decisions (unless gidget).
		var i, thing;
		for(i = 0; i < this.things.length; i++) {
		
			thing = this.things[i];
			
			var thingDecisions = thing.runtime.step();
			
			// Put the decisions in the table for the caller.
			if(isDef(thingDecisions))
				decisions.push(thingDecisions);
		
		}
	
		return decisions;
	
	};
	
};