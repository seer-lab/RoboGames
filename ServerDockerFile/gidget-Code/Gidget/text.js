// Contains all of the functions that generate the text for the user interface.
// The rationales for having a seperate file are many: this facilitates localization
// as well as making it easier to find and change text and modify text for the purposes
// of experimentation.

GIDGET.text = {

// *******************************************************
// *** R U N T I M E - M E S S A G E S *******************
// *******************************************************

	// The constant used to represent Gidget's sad state.
	SAD: 'sad',
	HAPPY: 'happy',

	Message: function(text, sound, emotion, functionToCall) {
	
		this.text = text;
		this.sound = sound;
		this.emotion = isDef(emotion) ? emotion : 'default';
		this.functionToCall = functionToCall;
		
		if(isDef(functionToCall) && !jQuery.isFunction(functionToCall)){
			console.error("Value passed to GIDGET.text.Message() is not a function but should be: " + functionToCall + ".");
		}
		
		return this;
	
	},

	if_true: function() {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("The $results(recent results) list is not empty, so continuing evaluation of list items.");
			
		return new GIDGET.text.Message("There are $results(recent results) in my memory, so I'll continue with these things.");
	
	},
	
	if_false: function() {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("The $results(recent results) list is empty, so ending this set of evaluations and moving on to next instructions.");
			
		return new GIDGET.text.Message("There aren't any $results(recent results) in my memory, so I'll end this set of evaluations and move on to the next part.");
	
	},
	
	if_popResults: function() {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Evaluation of the $results(recent results) complete. Removing from list before continuing.");
			
		return new GIDGET.text.Message("Before moving on, I'm going to empty the $results(recent results) from my memory since I'm done with them.");
	
	},
		
	is_positive: function(name, index, keyword, tag) {
		
		return new GIDGET.text.Message("The $results@" + index + "(" + name + ") " + keyword + " " + tag + ".");
	
	},

	is_negative: function(name, index, keyword, tag) {
			
		return new GIDGET.text.Message("The $results@" + index + "(" + name + ") " + keyword + " not " + tag + ".");
	
	},
	
	is_popResults: function() {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Finished with $results(this result). Removing from list.");
			
		return new GIDGET.text.Message("I'll remove $results(this result) from my memory since I'm done with it!");
	
	},
	
	is_newResults: function() {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Adding the $results(new results) to list.");
			
		return new GIDGET.text.Message("Now I'll add the $results(new results) to my memory.");
	
	},
		
	unknown_clearFocus: function() {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Clearing focus.", undefined, GIDGET.text.SAD);
			
		return new GIDGET.text.Message("Now I'll stop focusing on everything.", undefined, GIDGET.text.SAD);
	
	},

	unknown_clearResults: function() {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Clearing $results(recent results) from memory banks.", undefined, GIDGET.text.SAD);
			
		return new GIDGET.text.Message("I'm going to clear the $results(recent results) from my memory.", undefined, GIDGET.text.SAD);
	
	},	
	
	unknown_nextStep: function() {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Unknown command, so skipping to next step.", undefined, GIDGET.text.SAD);
			
		return new GIDGET.text.Message("I don't know what this is, so I'll just go on the next step.", undefined, GIDGET.text.SAD);
	
	},

	scan_success: function(name) {
	
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message(
				"<b>$scanned@0(" + this.capitalize(name) + ")</b> " + " added to the scan list.", 
				"scan");
			
		return new GIDGET.text.Message(
				"I <b>scanned</b> " + (name === 'gidget' ? "" : "a ") + "<b>$scanned@0(" + name + ")</b>. I'll add it to my scanned memory!",
				"scan");
	
	},

	name_success: function(name) {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Renaming $results@0(" + name + ")" + ".");
			
		return new GIDGET.text.Message("I renamed $results@0(" + name + ")" + ".");
	
	},

	go_dangerousStep: function(name, avoid) {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message(
				"Moving one space closer to destination, <b>$results@0(" + name + ")</b>, but may collide with <b>" + avoid + "</b>.",
				"goto");
		
		return new GIDGET.text.Message(
			"I'm going one step closer to the <b>$results@0(" + name + ")</b>, even though I might touch a <b>" + avoid + "</b>!",
			"goto");
	
	},

	go_step: function(name) {
	
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message(
				"Moving one space towards destination, <b>$results@0(" + name + ")</b>.",
				"goto");
		
		return new GIDGET.text.Message(
			"I'm going one step closer to the <b>$results@0(" + name + ")</b>!",
			"goto");
	
	},

	go_arrive: function(name) {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Arrived at destination, <b>$results@0(" + name + ")</b>.");
		
		return new GIDGET.text.Message("I made it to the <b>$results@0(" + name + ")</b>!");
	
	},

	go_noPath: function(name) {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("No valid path to $results@0(" + name + "). Aborting 'goto'.", undefined, GIDGET.text.SAD);
			
		return new GIDGET.text.Message("There's no way to get to the $results@0(" + name + "). So I guess I'm just going to skip the rest of this 'goto'.", undefined, GIDGET.text.SAD);
	
	},

	go_finished: function() {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Completed 'goto'.");
			
		return new GIDGET.text.Message("Phew! There's $results(nothing left to go to)!");
	
	},

	go_alreadyAt: function(name) {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Already at $results@0(" + name + ").");
			
		return new GIDGET.text.Message("I'm already at $results@0(" + name + ").");
	
	},

	analyze_success: function(name) {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message(
				"<b>$analyzed@0(" + this.capitalize(name) + ") analyzed</b>. Detailed information in memory bank.",
				"analyze");
		
		return new GIDGET.text.Message(
			"I <b>analyzed</b> the <b>$analyzed@0(" + name + ")</b>. I'll add it to my analyzed memory where you can see more information about it!",
			"analyze");
	
	},

	grab_success: function(name) {
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message(
				"<b>$grabbed@0(" + this.capitalize(name) + ")</b> grabbed. Added to memory bank.",
				"grab");
		
		return new GIDGET.text.Message(
			"I grabbed the <b>$grabbed@0(" + name + ")</b>. I'll add it to my memory!",
			"grab");
		
	},

	drop_success: function(name) {
	
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message(
				"Dropped $results@0(" + name + "). Removing from memory bank.",
				"drop");
			
		return new GIDGET.text.Message(
			"I dropped the $results@0(" + name + "). I'll remove it from my memory.",
			"drop");
	
	},

	focus_success: function(name) {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message(
				"Focusing on $results@0(next result), <b>" + name + "</b>",
				"focusIn");
			
		return new GIDGET.text.Message(
			"Okay, I'm going to concentrate on this <b>$results@0(individual " + name + ")</b> and add it to my focus memory.",
			"focusIn");
	
	},

	focus_failure: function() {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Failed to focus on next result.", undefined, GIDGET.text.SAD);
			
		return new GIDGET.text.Message("I'm supposed to focus on the next result, but $results(there isn't one)!", undefined, GIDGET.text.SAD);
	
	},

	unfocus_success: function() {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("<b>Focusing</b> terminated.");
			
		return new GIDGET.text.Message("Alright, I stopped <b>$focused(focusing)</b>.");
	
	},

	ask_waiting: function(name, action) {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Waiting for <b>" + name + "</b> to finish execution.");
			
		return new GIDGET.text.Message("Whee! I'm waiting for the <b>$thing(" + name + ")</b> to finish <b>" + this.add_ing(action) + "</b>.");
	
	},

	ask_finished: function(name) {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("<b>"+this.capitalize(name) + "</b> execution completed.");
			
		return new GIDGET.text.Message("Okay, the <b>$thing(" + name+ ")</b> is finished so I'm going to continue now.");
	
	},

	ask_noObject: function() {
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("ERROR: Nothing to <b>ask</b> by that name.", "error", GIDGET.text.SAD);
			
		return new GIDGET.text.Message("I couldn't find anything to <b>ask</b> by that name.", "errorExp", GIDGET.text.SAD);
	
	},

	ask_missingArguments: function(name, action, numberExpected, numberGiven) {
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("ERROR: Invalid <b>ask</b> syntax. " + name + " expects  <b>" + numberExpected + "</b> names, instead of <b>" + numberGiven + "</b> names. Skipping to next instructions.", "error", GIDGET.text.SAD);
			
		return new GIDGET.text.Message("Oh no... <b>" + name + "</b> knows how to <b>" + action + "</b>, but it wanted me to give it <b>" + numberExpected + "</b> names. I gave it <b>" + numberGiven + "</b> names. I don't know what to do! I guess I'll just skip this step.", "errorExp", GIDGET.text.SAD);
	
	},
		
	ask_begin: function(name, action) {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("<b>"+this.capitalize(action) + "</b> executing.");
			
		return new GIDGET.text.Message("Yay! <b>" + this.capitalize(name) + "</b> knows how to <b>" + action + "</b>. I'm going to tell it to do it.");
	
	},
	
	ask_unknownAction: function(name, action) {
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("ERROR: Invalid ask command. <b>" +this.capitalize(name)+ "</b> does not understand the action, <b>" + action + "</b>.", "error", GIDGET.text.SAD);
			
		return new GIDGET.text.Message("I <b>asked</b> <b>" + name + "</b> to <b>" + action + "</b> but it didn't know how! I don't know what to do!", "errorExp", GIDGET.text.SAD);
	
	},
	
	query: function(ast, name, scope) {
	
		var result, purposeText = "";
		switch(ast.type) {
			case "name": purposeText = "to <b>name</b>"; break;
			case "scan": purposeText = "to <b>scan</b>"; break;
			case "goto": purposeText = "to <b>go to</b>"; break;
			case "analyze": purposeText = "to <b>analyze</b>"; break;
			case "ask": purposeText = "to <b>ask</b>"; break;
			case "grab": purposeText = "to <b>grab</b>"; break;
			case "drop": purposeText = "to <b>drop</b>"; break;
			case "modify": purposeText = "to <b>modify</b>"; break;
			case "remove": purposeText = "to <b>remove</b>"; break;
			case "query": purposeText = "that were at the <b>same place</b>"; break;
			default: purposeText = "";
		}
		
		// "is" is the only ast.type that is checked separately from the switch statement above
		if (ast.type == "is") {
		
			if (isDef(ast) && isDef(ast.keyword))
				purposeText = "that <b>" + ast.keyword.text + " " + ast.tag.text + "</b>";
			else {
				if (GIDGET.experiment.isControl())
					purposeText = "that were at Gidget's <b>location</b>";
				else
					purposeText = "that were at <b>my location</b>";
			}
		}
		
		
		if (GIDGET.experiment.isControl())
			result = name === 'it' ? "<b>Focusing</b> on" : ast.type === "is" ? "Looked for any <b>" + name + "s</b> " + purposeText : "Looked for <b>" + name + "</b> " + purposeText;
		else
			result = name === 'it' ? "I'm currently <b>focused</b> on" : ast.type === "is" ? "I looked for any <b>" + name + "s</b> " + purposeText : "I looked for <b>" + name + "</b> " + purposeText;
		
		var i;
		if(scope.length > 0) {
			result += name === 'it' ? " " : " and detected ";
			result += ((name === 'gidget') ? "" : (scope.length === 1 && name !== 'it') ? "a " : (name === 'it') ? " " : "<b>" + scope.length + "</b> ");
			result += " <b>$results@(" + this.makePlural(name, scope) + ")</b>";
			
			/*
			for(i = 0; i < scope.length; i++)
				result = result + " $results@" + i + "(" + scope[i].name + ")" + (scope.length === 1 ? "" : i === scope.length - 1 ? "" : i === scope.length - 2 ? " and " : ",");
			*/
				
			if (GIDGET.experiment.isControl())
				return new GIDGET.text.Message(result + ". Added " + (scope.length === 1 ? "it" : "them") + " to the results list.");
			else
				return new GIDGET.text.Message(result + ". I'm going to add " + (scope.length === 1 ? "it" : "them") + " to my results list!");
		}
		else {
			if (GIDGET.experiment.isControl())
				return new GIDGET.text.Message(result + ", but that object does not exist in memory banks. Object may not yet be known, or it doesn't exist in this level.", "error", GIDGET.text.SAD);
			else
				return new GIDGET.text.Message(result + ", but I don't have that object in my memory. Either I don't know about it yet, or it doesn't exist in this level!", "errorExp", GIDGET.text.SAD);
		}

	},



// *******************************************************
// *** U S E R - I N T E R F A C E  - G E N E R A L ******
// *******************************************************
	
	editingDisabled: function() {
	
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("ERROR: Cannot modify code during program execution. Click the <b>" + this.finishExecutingButtonLabel() + "</b> button to end execution and then on the <b>retry this misssion</b> button to restart.", "error", GIDGET.text.SAD);
	
		return new GIDGET.text.Message("If you change my instructions while I'm doing them, I'm going to get really confused! You can make me stop by pressing the <b>" + this.finishExecutingButtonLabel() + "</b> button and then clicking the <b>retry this mission</b> button.", "errorExp", GIDGET.text.SAD);
		
	},
	
	finishExecutingButtonLabel: function() {
		return "to end";
	},

	noEnergy: function(){
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("CRITICAL ERROR: Energy depleted so cannot continue program execution. Retry?", "energyDown", GIDGET.text.SAD);
		
		return new GIDGET.text.Message("Oh no... I ran out of energy... Can we try again? I know with your help, I can succeed!", "energyDown", GIDGET.text.SAD);
		
	},
	
	aboutToStartGoals: function() {

		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Execution complete. Beginning evaluation of goals.");

  		return new GIDGET.text.Message("I finished executing my commands! Let's see if I accomplished all of my goals.");
	},

// *******************************************************
// *** U S E R - I N T E R F A C E  - M E M - L I S T ****
// *******************************************************


	memory_analyzed: function(iName, iActions, iTags) {
	
		var tags = "<br /><br />It has no special attributes.<br /><br />";
		var tagCount = 0;
		for(var tag in iTags) {
			if(iTags.hasOwnProperty(tag)) {
				tagCount++;
			}
		}

		if(tagCount > 0) {
			tags = "<br /><br />It is ";
			var index = tagCount;
			for(var tag in iTags) {
				if(iTags.hasOwnProperty(tag)) {
					// If this is the last one and there was one, just include the name.
					if(tagCount > 2 && index !== tagCount) tags = tags + ", ";
					// If there was more than one and this is the last one, prefix an 'and'
					if(tagCount >= 2 && index === 1) tags = tags + " and ";
					// Add the tag name.
					tags = tags + "<b>" + tag + "</b>";
					index--;	
				}
			}
			tags += ".<br />";
		}

		var actions;
		if (GIDGET.experiment.isControl())
			actions = "<br />It has no special functions.<br />";	
		else
			actions = "<br>There is nothing I can <b>ask</b> it to do.<br />";
		
		var actionCount = 0;
		for(var action in iActions)
			if(iActions.hasOwnProperty(action))
				actionCount++;
				
		if(actionCount > 0) {
			actions = "";
			for(var action in iActions) {
				if(iActions.hasOwnProperty(action)) {
					var arguments = iActions[action].arguments;
					var argString = "";
					if(arguments.length === 0) {
						if (GIDGET.experiment.isControl())
							argString = "it does not need any additional arguments";
						else
							argString = "I don't have to give it anything";
					}
					else {
						if (GIDGET.experiment.isControl())
							argString = "it takes ";
						else
							argString = "if I give it ";
						
						for(var index = 0; index < arguments.length; index++) {
							if(arguments.length > 2 && index != 0) argString = argString + ", ";
							if(arguments.length >= 2 && index === arguments.length - 1)	argString = argString + " and ";
							argString = argString + "<b>" + arguments[index] + "</b>";
						}
						//argString = argString + ".";
					
					}
					if (GIDGET.experiment.isControl()){
						actions += " It can be <b>asked</b> to <b>" + action + "</b> and " + argString + ".";
						}
					else{
						actions += " I can <b>ask</b> it to <b>" + action + "</b> " + argString + ".";
						}
					
				}
			}
		}
	
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Attributes of <b>" + iName + "</b> are now known. " + tags + actions);
			
		return new GIDGET.text.Message("I know all about <b>" + iName + "</b> because I <b>analyzed</b> it! " + tags + actions);
	
	},
		
	memory_unanalyzed: function(name) {
	
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("'" + name +"' has not been <b>analyzed</b>. Attributes unknown.");
			
		return new GIDGET.text.Message("I don't know anything about <b>" + name + "</b> because I haven't <b>analyzed</b> it yet.");
	
	},
	
	memory_unfocus: function() {
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("Returning back to program execution.");
		
		return new GIDGET.text.Message("Now where was I?");
	},

	modify: function(keyword, property, number) {
		
		var sound = undefined;
		
		if(property === 'energy') {
			if(keyword === 'raise') sound = "energyUp";
			else if(keyword === 'lower') sound = "energyDown";
			else if(keyword === 'set' && number === "0") sound = "energyDown";
		}
		
		return new GIDGET.text.Message("" + keyword + " " + property, sound);
		
	},

// *******************************************************
// *** U S E R - I N T E R F A C E  - G O A L S **********
// *******************************************************


	goal_checkSuccess: function() {
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("$results(Results) detected in memory banks, goal satisfied.", undefined, GIDGET.text.HAPPY);
		
		return new GIDGET.text.Message("There were $results(results) for this goal in my memory, so I succeeded!", undefined, GIDGET.text.HAPPY);
	},
	
	goal_checkFailure: function(){
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("ERROR: There were $results(no results) in the memory banks for this goal, so you <span class='runtimeReference'>failed this goal</span>.", undefined, GIDGET.text.SAD);
			
		return new GIDGET.text.Message("There were $results(no results) for this goal in my memory, so I didn't accomplish this goal!", undefined, GIDGET.text.SAD);
		
	},
	
	goal_finalSuccess: function() {
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("<span class='runtimeReference'>All goals</span> satisfied.", undefined, GIDGET.text.HAPPY);	
		
		return new GIDGET.text.Message("I accomplished <span class='runtimeReference'>all of my goals</span>! I never could have done it without you!", undefined, GIDGET.text.HAPPY);

	},
	
	goal_finalFailure: function(){
		
		if (GIDGET.experiment.isControl())
			return new GIDGET.text.Message("ERROR: <span class='runtimeReference'>Some goals</span> failed so mission is incomplete. Retry?", undefined, GIDGET.text.SAD);
			
		return new GIDGET.text.Message("I failed <span class='runtimeReference'>some of my goals</span> so I didn't complete this mission. I won't be able to figure this out without your help! Can you help me try again?", undefined, GIDGET.text.SAD);
		
	},

// *******************************************************
// *** P A R S E R - E R R O R - M E S S A G E S *********
// *******************************************************

		
	parser_unrecognizedCommand: function(token) {
	
		function showCommands() {
		
			GIDGET.ui.toggleCheatsheet(true);
			$('#cheatsheet').addClass('runtimeReference');
		
		}
	
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: \'" + token + "\' is an unrecognized command. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommands);
		} else {
			return new GIDGET.text.Message("" + token + " isn't one of the commands I know. I'm just going to go on.", "parserErrorExp", GIDGET.text.SAD, showCommands);
		}
		
	},

	parser_noCommandAfterComma: function() {
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing command after comma. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I saw a comma so I thought there would be a command after it, but there wasn't. I'll just keep going.", "parserErrorExp", GIDGET.text.SAD);
		}
	},
		
	parser_missingThingToName: function() {
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNATX ERROR: Missing thing to name. Must identify thing to name. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to name something, but I don't know what to name. I'm going to move on for now, but please tell me what you want to name next time since I'm so forgetful!", "parserErrorExp", GIDGET.text.SAD);
		}
	},
		
	parser_missingName: function() {
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing new name. Must state new name. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to name something, and I know what to name, but I don't know what to name it. I'm skipping this step for now since I can't figure it out, but can you tell me what to name it next time?", "parserErrorExp", GIDGET.text.SAD);
		}
	},
		
	parser_missingThingToScan: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('scan'); }
	
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to <b>scan</b>. Must identify thing to <b>scan</b>. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to <b>scan</b> something, but I don't know what. I'll move on for now, but can you make sure to tell me what to scan?", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},
		
	parser_missingThingToGoto: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('goto'); }

		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to <b>goto</b>. Must identify thing to <b>goto</b>. Skipping step", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to <b>goto</b> something, but I don't know what. This is difficult for me so I'll move on, but can you help me by telling me where to go next time?", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},
		
	parser_missingThingToAvoid: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('goto'); }
	
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to <b>avoid</b>. Must identify thing to <b>avoid</b>. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to goto something and <b>avoid</b> something, but I don't know what I'm supposed to <b>avoid</b>. I'm always getting so confused and bumping into things, so can you let me know what I should be avoiding next time?", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}	
	},
		
	parser_missingThingToAnalyze: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('analyze'); }

		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to <b>analyze</b>. Must identify thing to <b>analyze</b>. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to <b>analyze</b> something, but I don't know what. I get confused easily so I'll skip this step for now. Can you let me know what I should be analyzing next time?", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},

	parser_missingThingToAsk: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('ask'); }

		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to <b>ask</b>. Must identify thing to <b>ask</b>. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to <b>ask</b> something to do something, but I don't know what to <b>ask</b>. I'm going to skip this step for now, but I like talking to things, so can you let me know who or what I should be asking next time?", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},

	parser_missingTo: function() {

		function showCommand() { GIDGET.ui.highlightCommand('ask'); }	
	
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing 'to' statement. State 'to' between 'ask' command and thing. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("When I ask something to do something, I have to tell it 'to', but I didn't find that here so I'll skip it for now.", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}	
	},

	parser_missingAction: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('ask'); }

		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing <b>action</b>. State <b>action</b> for thing to do. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to <b>ask</b> something to do something, but I don't know what I'm <b>asking</b> it to do. I'll skip it so I don't confuse myself, but can you fix this instruction?", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},
	
	parser_unknownAction: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('ask'); }

		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Unknown action. Check object after <b>analyzing</b> it to see its list of commands. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("The object doesn't seem to know what the action is. Check the object after <b>analyzing</b> it to see its list of commands. I'll skip it so I don't confuse myself for now.", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},

	parser_missingThingToGrab: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('grab'); }

		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to <b>grab</b>. Must identify thing to <b>grab</b>. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to <b>grab</b> something, but I don't know what. Can you tell me?", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},

	parser_missingThingToDrop: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('drop'); }

		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to <b>drop</b>. Must identify thing to <b>drop</b>. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to <b>drop</b> something, but I don't know what to <b>drop</b>. Can you tell me? It's fun holding on to things, but I'll skip this for now.", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},

	parser_missingPredicate: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('if'); }
	
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Must state thing to check. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to check something, but I don't know to check. Can you tell me? I'll skip this for now.", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},
		
	parser_missingConditionalComma: function() {
	
		function showCommand() { GIDGET.ui.highlightCommand('if'); }

		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: A comma should come before any additional commands after an 'if'. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD, showCommand);
		} else {
			return new GIDGET.text.Message("I only know what to do when there's a comma after the test of an if so I'll skip this for now.", "parserErrorExp", GIDGET.text.SAD, showCommand);
		}
	},
		
	parser_missingThingToModify: function(keyword) {
	
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Must identify thing to <b>" + keyword + "</b>. Skipping step.","parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm suppose to <b>" + keyword + "</b> something, but I don't know what. Can you tell me? I'll skip this for now.", "parserErrorExp", GIDGET.text.SAD);
		}
	},
		
	parser_missingThingToAdd: function() {
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to add from map. Must identify thing to add. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm suppose to add something, but I don't know what. Can you tell me? I'm going to skip this for now.", "parserErrorExp", GIDGET.text.SAD);
		}
	},
				
	parser_missingThingToRemove: function() {
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to remove from map. Must identify the thing to remove. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to remove something, but I don't know what to remove. Can you tell me?  I'll skip this for now.", "parserErrorExp", GIDGET.text.SAD);
		}			
	},
		
	parser_missingAndPredicate: function() {
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to check. Must identify the thing to check. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm supposed to check something, but I don't know to check. Can you tell me? I'll just skip this step.", "parserErrorExp", GIDGET.text.SAD);
		}
	},
	
	parser_missingTag: function() {
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing tag of thing to check. Must state the tag to check. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm suppose to see if this has some tag, but I don't know which tag. Can you tell me? I'll just go on for now.", "parserErrorExp", GIDGET.text.SAD);
		}
	},
	
	parser_missingQueryName: function() {
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to find. Must identify the thing to check. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm suppose to see find something with a certain name, but I don't know the name of things to find., but I don't know which tag. Can you tell me? I'll skip this step for now.", "parserErrorExp", GIDGET.text.SAD);
		}
	},
	
	parser_missingOn: function() {
		
		if (GIDGET.experiment.isControl()) {
			return new GIDGET.text.Message("SYNTAX ERROR: Missing name of thing to check on other thing. Must identify the thing to check. Skipping step.", "parserErrorCtrl", GIDGET.text.SAD);
		} else {
			return new GIDGET.text.Message("I know I'm suppose to find something on something else, but I don't know what something else. Can you tell me? I'll skip this for now.", "parserErrorExp", GIDGET.text.SAD);
		}
	},
	

// *******************************************************
// *******************************************************
// *******************************************************

	capitalize: function(str) {
    	return str.charAt(0).toUpperCase() + str.slice(1);
	},
	
	makePlural: function(str, instances ){
	
		if (instances.length > 1 && str.charAt(str.length - 1) !== 's')
			return str + "s";
		else
			return str;
	},
	
	add_ing: function(str){
	
		if (str.length >= 3){
			//check -ie suffix
			if (str.charAt(str.length-2) === "i" && str.charAt(str.length-1) === "e")
				return str.substring(0, str.length-1) + "ing";
			//check if consonant + 'e' suffix
			if (!this.isVowel(str.charAt(str.length-2)) && str.charAt(str.length-1) === "e" )
				return str.substring(0, str.length-1) + "ing";
			// check for suffix: consonant + vowel + consonant
			if (!this.isVowel(str.charAt(str.length-3)) && this.isVowel(str.charAt(str.length-2)) && !this.isVowel(str.charAt(str.length-3)))
				return str + str.charAt(str.length - 1) + "ing";
		}
		return str;
	},
	
	isVowel: function(a) {
	
		if(a.length == 1 && (a == 'a' || a == 'e' || a == 'i' || a == 'o' || a == 'u'))
			return true;
		else return false;
  	},

};