/*

GIDGET Grammar

PROGRAM 	:: { COMMAND UNKNOWN }*
UNKNOWN 	:: token* eol
SPEAK		:: say token*
COMMAND 	:: NAME | IF | SCAN | GOTO | ANALYZE | TELL | GRAB | DROP | MODIFY | ADD | REMOVE | UNKNOWN
NAME		:: name QUERY STRING [FOCUS]
SCAN 		:: scan QUERY [FOCUS]
GOTO 		:: goto QUERY [avoid STRING] [FOCUS]
ANALYZE 	:: analyze QUERY [FOCUS]
GRAB 		:: grab QUERY [FOCUS]
DROP 		:: drop QUERY [FOCUS]
ASK 		:: ask QUERY STRING QUERY* [FOCUS]
MODIFY 		:: (raise | lower | change) QUERY ( energy | height ) INT [, COMMAND]
ADD			:: add NAME
REMOVE		:: remove QUERY
FOCUS		:: , COMMAND
IF 			:: (if|when) PREDICATE, COMMAND
PREDICATE 	:: IS [and PREDICATE]
IS			:: QUERY [(is | isn't) STRING]
QUERY 		:: it | FILTER* STRING [on QUERY]
FILTER		:: nearest | first | second | third | one | two | three | last | grabbed | reachable | scanned | analyzed | level | over | focused
STRING 		:: [a-zA-Z]+
GOAL		:: PREDICATE+

*/

GIDGET.parser = {

	// I started to work on eliminating string literals in the parser, but have barely started.
	KEYWORDS: {
	
		ACTION: "ask"
	
	},

	// Represents an AST node for an unparsable sequence of tokens
	Unknown: function(tokens, message) {
		
		this.type = 'unknown';
		this.tokens = tokens;
		this.message = message;
		this.serialize = function() {
				
			return [ new GIDGET.runtime.Step_UNKNOWN(this, this.tokens[0], this.tokens, this.message) ];
				
		}
	
	},

	Token: function(text, kind, line, index) {

		this.text = text;
		this.kind = kind;
		this.line = line;
		this.index = index;

	},

	TokenStream: function(code) {
	
		if(!isDef(code))
			console.error("Somehow, the code sent to this token stream is not a string.");
	
		this.tokens = [];
		
		this.hasMore = function() { return this.tokens.length > 0; };
		this.peek = function() { return this.hasMore() ? this.tokens[0].text : undefined; };
		this.eat = function() { return this.hasMore() && this.tokens.splice(0, 1)[0]; };
		this.nextIs = function(text) { return this.hasMore() && this.peek().toLowerCase() === text.toLowerCase(); };
		this.eol = function() { return this.hasMore() && this.peek().match(/\r\n|\r|\n/); };
		this.nextIsComma = function() { return this.hasMore() && this.peek() == ','; };
		this.nextIsString = function() { return this.hasMore() && this.tokens[0].kind === 'string'; };
		this.nextIsEndOfCommand = function() { return this.hasMore() && (this.nextIsComma() || this.eol()); };
		
		// Returns an array of all of the tokens up until the next comma or end of line.
		this.eatLine = function() {
		
			var tokens = [];
			while(!this.eol())
				tokens.push(this.eat());
			// If we stopped at an eol, continue eating them and tossing them away.
			while(this.eol())
				tokens.push(this.eat());
			return tokens;
		
		}
		
		// First split the program into [a-zA-Z0-9] words separated by spaces, newlines, apostrophes, and commas
		var lines = code.split(/\r\n|\r|\n/);
		var lineNumber;
		var line;
		for(lineNumber = 0; lineNumber < lines.length; lineNumber++) {
				
			line = lines[lineNumber];
			var charIndex, char;
			var id = "";
			for(charIndex = 0; charIndex < line.length; charIndex++) {
				
				char = line.charAt(charIndex);

				// If it's a space, add a space and whatever was accumulated.
				if(char.match(/\s/)) {
					if(id.length > 0)
						this.tokens.push(new GIDGET.parser.Token(id, "string", lineNumber, this.tokens.length));
					id = "";
				}
				// If it's a comma, add the accumulated id if necessary and then add a comma,
				// resetting the accumulated id.
				else if(char === ',') {
				
					if(id.length > 0)
						this.tokens.push(new GIDGET.parser.Token(id, "string", lineNumber, this.tokens.length));
					this.tokens.push(new GIDGET.parser.Token(',', "comma", lineNumber, this.tokens.length));
					id = "";

				}
				// Otherwise, just accumulate characters for the id.
				else
					id = id + char;
			
			}

			// If there's text accumulated for the id, generate a token for it.			
			if(id.length > 0)
				this.tokens.push(new GIDGET.parser.Token(id, "string", lineNumber, this.tokens.length));
			
			// End the line.
			this.tokens.push(new GIDGET.parser.Token('\n', "eol", lineNumber, this.tokens.length));
				
		}

	},

	parseScript: function(code) {
	
		var tokens = new GIDGET.parser.TokenStream(code);
		var ast = this.parseProgram(tokens);
		var steps = ast.serialize();
		
		return steps;
	
	},

	parseGoal: function(code) {
	
		var tokens = new GIDGET.parser.TokenStream(code);
		var ast = this.parsePredicate(tokens);
				
		var steps = ast.serialize();
		
		var unknown = [];
		while(tokens.hasMore()) {
			if(tokens.eol()) tokens.eat();
			else if(tokens.peek() === undefined) tokens.eat();
			else unknown.push(tokens.eat());
		}
			
		if(unknown.length > 0) {
		
			steps.push(new GIDGET.runtime.Step_UNKNOWN(this, unknown[0], unknown))
		
		}

		return steps;
	
	},

	// PROGRAM :: { COMMAND UNKNOWN }*
	// A series of end of line terminated commands.
	parseProgram: function(tokenStream) {
	
		var program = {
			type: 'program',
			commands: [],
			serialize: function() {

				var steps = [];
				var i;
				for(i = 0; i < this.commands.length; i++)
					steps = steps.concat(this.commands[i].serialize());
					
				return steps;

			}
		}
		
		var command;
		var unknown;
	
		// Are there any more tokens?
		while(tokenStream.hasMore()) {

			// Eat all blank lines and empty strings
			while(tokenStream.hasMore() && (tokenStream.eol() || tokenStream.nextIs(''))) 
				tokenStream.eat();
				
			// If there are more tokens, eat commands.
			if(tokenStream.hasMore()) {
		
				// Eat the command and the tokens it expects
				command = this.parseCommand(tokenStream);

				// Parse command always returns something.				
				program.commands.push(command);

				// Eat all of the trailing new lines.
				while(tokenStream.eol())
					tokenStream.eat();
					
			}
	
		}	
		
		return program;
	
	},
		
	// COMMAND :: IF | SCAN | GOTO | ANALYZE | TELL | GRAB | DROP | UNKNOWN
	// One of the above kinds of commands, or a list of unknown end of line terminated tokens.
	parseCommand: function(tokenStream) {
		
		var unknown = [];

		if(tokenStream.nextIs('if') || tokenStream.nextIs('when')) {
			return this.parseIf(tokenStream);
		}
		else if(tokenStream.nextIs('say')) {
			return this.parseSpeak(tokenStream);		
		}
		else if(tokenStream.nextIs('scan')) {
			return this.parseScan(tokenStream);		
		}
		else if(tokenStream.nextIs('name')) {
			return this.parseName(tokenStream);		
		}
		else if(tokenStream.nextIs('goto')) {
			return this.parseGoto(tokenStream);		
		}
		else if(tokenStream.nextIs('analyze')) {
			return this.parseAnalyze(tokenStream);
		}
		else if(tokenStream.nextIs(this.KEYWORDS.ACTION)) {
			return this.parseAsk(tokenStream);		
		}
		else if(tokenStream.nextIs('grab')) {
			return this.parseGrab(tokenStream);		
		}
		else if(tokenStream.nextIs('drop')) {
			return this.parseDrop(tokenStream);		
		}
		else if(tokenStream.nextIs('add')) {
			return this.parseAdd(tokenStream);		
		}
		else if(tokenStream.nextIs('remove')) {
			return this.parseRemove(tokenStream);		
		}
		else if(tokenStream.nextIs('raise') || tokenStream.nextIs('lower') || tokenStream.nextIs('set')) {
			return this.parseModify(tokenStream);		
		}
		
		// If we made it this far, we didn't recognize a command. Eat the rest of the line.
		var token = tokenStream.peek();
		
		// If there was no recognizable command keyword, eat (and ignore) all the tokens up until the newline
		return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_unrecognizedCommand(token));
	
	},

	// FOCUS :: , COMMAND
	parseFocus: function(tokenStream) {

		var focus = {
			type: 'focus',
			comma: undefined,
			command: undefined,
			
			serialize: function() {

				var steps = [];	

				// Scope execution to the Thing just scanned.
				steps.push(new GIDGET.runtime.Step_SCOPE(this, this.comma));
				// Execute the command.
				steps = steps.concat(this.command.serialize());
				// Return to the original query scope.
				steps.push(new GIDGET.runtime.Step_UNSCOPE(this, this.comma));
					
				return steps;
			
			}
		};

		// Everything that calls parseFocus() needs be prepared for an undefined!
		if(!tokenStream.nextIs(','))
			return undefined;
		
		// eat ','
		focus.comma = tokenStream.eat();

		// If the next token is not a string (but a comma or newline), return an unknown.
		if(!tokenStream.nextIsString()) {
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_noCommandAfterComma());
		}

		// eat the name to query
		focus.command = this.parseCommand(tokenStream);
		
		return focus;

	},

	// Just a convenience function for code reuse.
	createFocusStructure: function(ast, query, focus, step) {
	
		var steps = [];
		
		steps = steps.concat(query.serialize());

		var commandSteps = isDef(focus) ? focus.serialize() : [];
		steps.push(step);
		step.offset = commandSteps.length + 1;
		steps = steps.concat(commandSteps);
		steps.push(new GIDGET.runtime.Step_NEXT(ast, query.name, -(commandSteps.length + 1)));
	
		return steps;
	
	
	},

	// NAME :: name QUERY STRING [FOCUS]
	// Renames all things returned in the query with the given name.
	parseName: function(tokenStream) {
	
		var name = {
			type: 'name',
			keyword: undefined,
			name: undefined,
			focus: undefined,
			
			serialize: function() {

				if(!isDef(this.query))
					return [new GIDGET.runtime.Step_UNKNOWN(this, this.keyword, [this.keyword])];

				return GIDGET.parser.createFocusStructure(this, this.query, this.focus, new GIDGET.runtime.Step_NAME(this, this.name, this.name));
			
			}
		};
		
		// eat 'name'
		name.keyword = tokenStream.eat();

		// If this is the end of the line, explain that to name something, we need a query.
		if(tokenStream.eol())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToName());

		// eat the name to query
		name.query = this.parseQuery(tokenStream, name, []);

		// If there was no parsable query, return the unknown AST.
		if(name.query.type === 'unknown')
			return name.query;

		// Is the next thing is not a string, then explain.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingName());

		// eat the name to query
		name.name = tokenStream.eat();

		// eat the optional focus		
		name.focus = this.parseFocus(tokenStream);
		
		return name;

	},
	
	
	// SCAN :: scan QUERY [FOCUS]
	// Finds all Things with names equal to the NAME (without a trailing s) and scans the ones reachable through Gidget's GOTO algorithm,
	// adding each scanned Thing to Gidget's scanned list. If there is no focus, the scope of the search is all objects at Gidget's level.
	// If no query is supplied, everything at Gidget's level is eligible to be scanned. An optional command can be included, focusing
	// on each individual object that is successfully scanned.
	parseScan: function(tokenStream) {
	
		var scan = {
			type: 'scan',
			keyword: undefined,
			query: undefined,
			focus: undefined,
			
			serialize: function() {

				return GIDGET.parser.createFocusStructure(this, this.query, this.focus, new GIDGET.runtime.Step_SCAN(this, this.keyword));

			}
		};
	
		// eat 'scan'
		scan.keyword = tokenStream.eat();

		// If the next thing is not the start of a query, eat the line.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToScan());

		// eat the name to query
		scan.query = this.parseQuery(tokenStream, scan, [ 'reachable', 'level' ] );

		// If there was no parsable query, return the unknown AST.
		if(scan.query.type === 'unknown')
			return scan.query;

		// eat the optional focus
		scan.focus = this.parseFocus(tokenStream);
		
		return scan;
	
	},
	
	// GOTO :: goto QUERY [avoid STRING] [FOCUS]
	// Moves Gidget to all scanned objects matching the given name.
	// If the optional query isn't supplied, the query returns everything scanned, or if Gidget is focused on something, just
	// the focused thing. The optional command operates on each match.
	parseGoto: function(tokenStream) {
	
		var go = {
			type: 'goto',
			keyword: undefined,
			query: undefined,
			avoid: undefined,
			focus: undefined,
			
			serialize: function() {

				return GIDGET.parser.createFocusStructure(this, this.query, this.focus, new GIDGET.runtime.Step_GO(this, this.keyword, this.avoid));
			
			}
			
		}
	
		// eat 'goto'
		go.keyword = tokenStream.eat();

		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToGoto());

		// eat the query
		go.query = this.parseQuery(tokenStream, go, [ 'scanned' ]);

		// If there was no parsable query, return the unknown AST.
		if(go.query.type === 'unknown')
			return go.query;

		// If the next thing is an avoid, parse it.
		if(tokenStream.nextIs('avoid')) {

			// eat 'avoid'
			tokenStream.eat();

			// There must be a string to avoid.			
			if(!tokenStream.nextIsString())
				return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToAvoid());

			// Otherwise, eat the thing to avoid.
			go.avoid = tokenStream.eat();
		
		}
		
		// Parse the optional focus.		
		go.focus = this.parseFocus(tokenStream);
		
		return go;
	
	},
		
	// ANALYZE 	:: analyze QUERY [FOCUS]	
	// Finds all Things with names equal to the NAME (without a trailing s) that are at the same location as Gidget and at the same level.
	// If it's name matches, it adds the Thing to the analyzed list. If no query is supplied, everything at Gidget's location and level is analyzed.
	// An optional command can be included, focusing on each individual object that is successfully scanned.
	parseAnalyze: function(tokenStream) {

		var analyze = {
			type: "analyze",
			keyword: undefined,
			query: undefined,
			focus: undefined,
			
			serialize: function() {
			
				return GIDGET.parser.createFocusStructure(this, this.query, this.focus, new GIDGET.runtime.Step_ANALYZE(this, this.keyword));
			
			}
		};
			
		// eat 'analyze'
		analyze.keyword = tokenStream.eat();

		// If this is the end of the line, just return the broad scan
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToAnalyze());

		// eat the name to query
		analyze.query = this.parseQuery(tokenStream, analyze, [ 'over', 'level' ]);

		// If there was no parsable query, return the unknown AST.
		if(analyze.query.type === 'unknown')
			return analyze.query;

		// Parse the optional focus.
		analyze.focus = this.parseFocus(tokenStream);
		
		return analyze;

	},
		
	// ASK :: ask QUERY to STRING QUERY* [FOCUS]
	// calls action STRING on OBJECTS returned by QUERY
	parseAsk: function(tokenStream) {
	
		var ask = {
			type: 'ask',
			keyword: undefined,
			query: undefined,
			action: undefined,
			arguments: [],
			focus: undefined,
			
			serialize: function() {

				var steps = [];

				// Get all of the Things to make do something
				steps = steps.concat(this.query.serialize());

				// Serialize all of the command steps, if any, but don't add them yet.	
				var commandSteps = isDef(this.focus) ? this.focus.serialize() : [];

				// Query the arguments
				var argumentStepCount = steps.length;
				for(i = 0; i < this.arguments.length; i++) {
					steps = steps.concat(this.arguments[i].serialize());
				}
				argumentStepCount = steps.length - argumentStepCount;

				var ask = new GIDGET.runtime.Step_ASK(this, this.action, this.query, this.action, this.arguments);

				// If there aren't any results, it should skip over the commands.
				ask.offset = commandSteps.length + 1;

				// Make it do something				
				steps.push(ask);
				
				// Then it should do the commands
				steps = steps.concat(commandSteps);
				
				// Then it should make the next Thing do something, skipping back to the arguments.
				steps.push(new GIDGET.runtime.Step_NEXT(this, this.query.name, -(commandSteps.length + 1 + argumentStepCount)));
			
				return steps;	
			
			}
			
		};
	
		// eat 'ask'
		ask.keyword = tokenStream.eat();

		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToAsk());

		// eat query
		ask.query = this.parseQuery(tokenStream, ask, [ 'over', 'analyzed' ]);

		// If there was no parsable query, return the unknown AST.
		if(ask.query.type === 'unknown')
			return ask.query;

		// eat 'to'
		if(!tokenStream.nextIs('to'))
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingTo());

		// eat 'to'
		tokenStream.eat();

		// There must be an action next.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingAction());

		// eat action name.
		ask.action = tokenStream.eat();

		// eat zero or more arguments
		while(tokenStream.nextIsString()) {
		
			var query = this.parseQuery(tokenStream, ask, [ 'scanned' ]);

			// If there was no parsable query, return the unknown AST.
			if(query.type === 'unknown')
				return query;

			ask.arguments.push(query);
			
		}

		// Eat the optional focus.			
		ask.focus = this.parseFocus(tokenStream);
		
		return ask;

	},
	
	// GRAB :: grab QUERY [FOCUS]
	// Constrain location of queried Things at the Thing's location to the Thing's location, also
	// adding the Things to the Thing's grabbed list.
	parseGrab: function(tokenStream) {

		var grab = {
			type: 'grab',
			keyword: undefined,
			query: undefined,
			focus: undefined,
			
			serialize: function() {

				return GIDGET.parser.createFocusStructure(this, this.query, this.focus, new GIDGET.runtime.Step_GRAB(this, this.keyword));
			
			}
		}
	
		// eat 'grab'
		grab.keyword = tokenStream.eat();

		// There must be a query next.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToGrab());		

		// Eat the query.
		grab.query = this.parseQuery(tokenStream, grab, ['over']);

		// If there was no parsable query, return the unknown AST.
		if(grab.query.type === 'unknown')
			return grab.query;

		// Eat the optional focus.
		grab.focus = this.parseFocus(tokenStream);

		return grab;

	},
	
	// DROP :: drop QUERY [FOCUS]
	// Unconstrain location of returned objects from Gidget's location. If a query isn't supplied,
	// everything in the current query scope that is grabbed is dropped.
	parseDrop: function(tokenStream) {

		var drop = {
			type: 'drop',
			keyword: undefined,
			query: undefined,
			focus: undefined,
			
			serialize: function() {
			
				return GIDGET.parser.createFocusStructure(this, this.query, this.focus, new GIDGET.runtime.Step_DROP(this, this.keyword));
			
			}

		}
	
		// eat 'drop'
		drop.keyword = tokenStream.eat();

		// There must be a query next.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToDrop());		

		// Eat the query		
		drop.query = this.parseQuery(tokenStream, drop, ['grabbed']);

		// If there was no parsable query, return the unknown AST.
		if(drop.query.type === 'unknown')
			return drop.query;
			
		// Eat the optional focus.
		drop.focus = this.parseFocus(tokenStream);
			
		return drop;

	},
	
	// IF :: (if|when) PREDICATE, COMMAND
	parseIf: function(tokenStream) {
	
		var conditional = {
			type: 'conditional',
			test: undefined,
			predicate: undefined,
			then: undefined,

			serialize: function() {

				var steps = [];

				steps = steps.concat(this.predicate.serialize());
				
				var thenSteps = this.then.serialize(steps);

				steps.push(new GIDGET.runtime.Step_IF(this, this.test, thenSteps.length + 1));
				steps = steps.concat(thenSteps);

				// If this is a conditional, return the steps
				if(this.test.text === 'if')
					return steps;
				// Otherwise, return a when, which will execute every step.
				else {				
					return [ new GIDGET.runtime.Step_WHEN(this, this.test, steps) ];
				}
			
			}

		};
	
		// Eat 'if'
		conditional.test = tokenStream.eat();

		// There must be a predicate next.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingPredicate());		

		// Eat the predicate.
		conditional.predicate = this.parsePredicate(tokenStream);

		// If the predicate didn't parse, return it.
		if(conditional.predicate.type === 'unknown')
			return conditional.predicate;

		// There must be a comma then a command.
		if(!tokenStream.nextIsComma())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingConditionalComma());

		// Eat comma
		tokenStream.eat();
		
		// Eat command.
		conditional.then = this.parseCommand(tokenStream);
	
		return conditional;
			
	},
	
	// MODIFY :: (raise | lower | set) QUERY ( energy | height ) [INT] [, COMMAND]
	// Constrain location of queried Things at the Thing's location to the Thing's location, also
	// adding the Things to the Thing's grabbed list.
	parseModify: function(tokenStream) {

		var modify = {
			type: 'modify',
			keyword: undefined,
			query: undefined,
			property: undefined,
			number: undefined,
			
			serialize: function() {

				return GIDGET.parser.createFocusStructure(this, this.query, this.focus, new GIDGET.runtime.Step_MODIFY(this, this.keyword, this.keyword, this.property, this.number));
			
			}
		}
	
		// eat one of the valid keywords
		modify.keyword = tokenStream.eat();
		modify.keyword.text = modify.keyword.text.toLowerCase();

		// Try to parse a query
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToModify(modify.keyword.text));
		
		// No implicit constraints on the query
		modify.query = this.parseQuery(tokenStream, modify);

		// If there was no parsable query, return the unknown AST.
		if(modify.query.type === 'unknown')
			return modify.query;

		// There must be a thing to modify next.
		if(!tokenStream.nextIsString())
			return modify;

		// Eat the thing to modify.
		modify.property = tokenStream.eat();
		modify.property.text = modify.property.text.toLowerCase();

		// There must be an amount
		if(!isNaN(parseInt(tokenStream.peek())))
			modify.number = parseInt(tokenStream.eat().text);

		// Eat the optional focus.
		modify.focus = this.parseFocus(tokenStream);

		return modify;

	},
	
	// SAY :: say token*
	parseSpeak: function(tokenStream) {

		var ast = {
			type: 'say',
			keyword: undefined,
			message: undefined,
			
			serialize: function() {

				return [ new GIDGET.runtime.Step_SAY(this, this.keyword, this.message) ];
			
			}
		}
	
		// eat the keyword
		ast.keyword = tokenStream.eat();

		var message = "";
		var token;
		while(!tokenStream.eol()) {
			token = tokenStream.eat().text;
			message = message + token + (tokenStream.hasMore() && tokenStream.nextIsComma() ? "" : " ");
		}
		
		ast.message = message;

		return ast;

	},
	
	// ADD :: add NAME [FOCUS]
	parseAdd: function(tokenStream) {

		var ast = {
			type: 'add',
			keyword: undefined,
			name: undefined,
			focus: undefined,
			
			serialize: function() {

				var steps = [ new GIDGET.runtime.Step_ADD(this, this.keyword, this.name) ];

				var commandSteps = isDef(this.focus) ? this.focus.serialize() : [];
				steps = steps.concat(commandSteps);
				
				return steps;
			
			}
		}
	
		// eat the keyword
		ast.keyword = tokenStream.eat();

		// Try to parse a query
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToAdd());
		
		// No implicit constraints on the query
		ast.name = tokenStream.eat();

		// Eat the optional focus.
		ast.focus = this.parseFocus(tokenStream);

		return ast;

	},
	
	// REMOVE :: remove QUERY [FOCUS]
	parseRemove: function(tokenStream) {

		var ast = {
			type: 'remove',
			keyword: undefined,
			query: undefined,
			focus: undefined,
			
			serialize: function() {
			
				return GIDGET.parser.createFocusStructure(this, this.query, this.focus, new GIDGET.runtime.Step_REMOVE(this, this.keyword));
			
			}

		}
	
		// eat 'remove'
		ast.keyword = tokenStream.eat();

		// There must be a query next.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingThingToRemove());		

		// Eat the query		
		ast.query = this.parseQuery(tokenStream, ast, ['over']);

		// If there was no parsable query, return the unknown AST.
		if(ast.query.type === 'unknown')
			return ast.query;
			
		// Eat the optional focus.
		ast.focus = this.parseFocus(tokenStream);

		return ast;

	},
	
	// PREDICATE :: IS [and PREDICATE]
	parsePredicate: function(tokenStream) {

		var and = {
			type: 'and',
			left: undefined,
			keyword: undefined,
			right: undefined,

			serialize: function() {
			
				var steps = [];

				steps = steps.concat(this.left.serialize());
				
				if(isDef(this.right)) {
					steps = steps.concat(this.right.serialize());
					steps.push(new GIDGET.runtime.Step_AND(this, this.keyword));
				}
								
				return steps;
			
			}
		}
		
		and.left = this.parseIs(tokenStream);
		
		// If the is didn't parse, return the unknown AST.
		if(and.left.type === 'unknown')
			return and.left;
		
		// If there's no and, we're done.
		if(!tokenStream.nextIs('and'))
			return and;

		// If there is, eat the keyword.
		and.keyword = tokenStream.eat();

		// There must be a predicate next.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingAndPredicate());		

		// Eat the predicate.		
		and.right = this.parsePredicate(tokenStream);

		// If the predicate didn't parse, return the unknown AST.
		if(and.right.type === 'unknown')
			return and.right;
		
		return and;		
				
	},

	// IS :: QUERY [(is | isn't) STRING]
	parseIs: function(tokenStream) {

		var predicate = {
			type: 'is',
			query: undefined,
			keyword: undefined,
			tag: undefined,

			serialize: function() {
			
				var steps = [];

				steps = steps.concat(this.query.serialize());
				
				// If there's a predicate, filter the results returned by the query by the given keyword
				steps.push(new GIDGET.runtime.Step_IS(this, this.keyword, this.keyword, this.tag));

				return steps;
			
			}
		};

		predicate.query = this.parseQuery(tokenStream, predicate);

		// If there was no parsable query, return the unknown AST.
		if(predicate.query.type === 'unknown')
			return predicate.query;
		
		// There are only four valid predicates. If none are found, assume none were meant and just return the query.
		if(!tokenStream.peek().match(/is|isn't|are|aren't/))
			return predicate.query;
		
		// Eat the valid predicate.
		predicate.keyword = tokenStream.eat();

		// There must be a tag next.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingTag());

		// Eat the tag.		
		predicate.tag = tokenStream.eat();
		
		return predicate;	
	
	},

	// QUERY :: it | ( nearest | first | second | third | one | two | three | last | grabbed | reachable | scanned | analyzed | level | over | focused )* STRING [on QUERY]
	parseQuery: function(tokenStream, parent, constraints) {

		// If no constraint array was supplied, create an empty one.
		if(!isDef(constraints))
			constraints = [];

		var query = {
			type: 'query',
			constraints: constraints,
			name: undefined,
			on: undefined,
			parent: parent,
			
			serialize: function() {

				var steps = [];

				if(isDef(this.on)) {
					steps = steps.concat(this.on.serialize());				
				}

				steps.push(new GIDGET.runtime.Step_QUERY(this, this.parent, this.name, this.name, this.constraints, isDef(this.on)));
				
				return steps;
			
			}
		};
		
		// We start by assuming that there is some string token next, assuming the caller has checked.
		
		// Is the first token the keyword 'it'? If so, we're done.
		if(tokenStream.nextIs('it')) {
			query.name = tokenStream.eat();
			constraints.push('focused');
			return query;
		}

		// Otherwise, we look for zero or more query filters.
		while(tokenStream.tokens[0].text.match(/nearest|first|second|third|one|two|three|last|grabbed|scanned|analyzed|level|over|focused/i)) {
		
			query.constraints.push(tokenStream.eat().text.toLowerCase());
			
		}

		// There must be a name next.
		if(!tokenStream.nextIsString())
			return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingQueryName());

		// Eat the query name.
		query.name = tokenStream.eat();
				
		// Check for the optional on operator.
		if(tokenStream.nextIs('on')) {

			// Eat the keyword on.
			tokenStream.eat();

			if(!tokenStream.nextIsString())
				return new this.Unknown(tokenStream.eatLine(), GIDGET.text.parser_missingOn());
			
			query.on = this.parseQuery(tokenStream, parent);
			
			// If the query was unknown, return it instead.
			if(isDef(query.on) && query.on.type === 'unknown')
				return query.on.type;
			
		}		

		return query;
	
	}
	
}