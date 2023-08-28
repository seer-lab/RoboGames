GIDGET.levels = {

	// *******************************************************
	
	parser_unrecognizedCommand: function() {

		// ----- G - C O D E -----
	
		var code = 
			"catch bat";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("cat on crate is infected");
		
		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_unrecognizedCommand");
		
		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bat", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},	
	// *******************************************************
	
	parser_noCommandAfterComma: function() {

		// ----- G - C O D E -----
	
		var code = 
			"scan piglet, goto it,\n" +
			"scan crate,";
		
		var world = new GIDGET.World([10,10], [6,6], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("piglet is blue");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_noCommandAfterComma");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "piglet", 7, 8, "green", [] , {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},		
	
	// *******************************************************
	
	parser_missingThingToName: function() {

		// ----- G - C O D E -----
	
		var code = 
			"name\n";
		
		var world = new GIDGET.World([10,10], [5,6], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToName");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},	
	
		// *******************************************************
	
	parser_missingName: function() {

		// ----- G - C O D E -----
	
		var code = 
			"scan cat, goto it, name it\n" +
			"name cat\n" +
			"analyze cat";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingName");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "cat", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},	
	
	// *******************************************************
	
	parser_missingThingToScan: function() {

		// ----- G - C O D E -----
	
		var code = 
			"scan";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("scanned cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToScan");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "cat", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},

	
	// *******************************************************
	
	parser_missingThingToGoto: function() {

		// ----- G - C O D E -----
	
		var code = 
			"scan cats\n" +
			"goto\n" +
			"analyze cat";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToGoto");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "cat", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	// *******************************************************
	
	parser_missingThingToAvoid: function() {

		// ----- G - C O D E -----
	
		var code = 
			"scan cats, goto it avoid";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToAvoid");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	// *******************************************************
	
	parser_missingThingToAnalyze: function() {

		// ----- G - C O D E -----
	
		var code = 
			"analyze";
		
		var world = new GIDGET.World([10,10], [5,6], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToAnalyze");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	// *******************************************************
	
	parser_missingThingToAsk: function() {

		// ----- G - C O D E -----
	
		var code = 
			"scan bird, analyze it, ask to fly\n" +
			"ask\n";
		
		var world = new GIDGET.World([10,10], [1,8], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToAsk");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	//** ASK TO. IF PREVIOUS STRING is "TO" it should say something slightly different 
	
	
	
	// *******************************************************
	
	parser_missingTo: function() {

		// ----- G - C O D E -----
	
		var code = 
			"analyze bird\n" +
			"ask bird fly";
		
		var world = new GIDGET.World([10,10], [1,8], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingTo");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	// *******************************************************
	
	parser_missingAction: function() {

		// ----- G - C O D E -----
	
		var code = 
			//"analyze bird\n" +
			"ask bird to";
		
		var world = new GIDGET.World([10,10], [1,8], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingAction");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	// *******************************************************
	
	parser_missingThingToGrab: function() {

		// ----- G - C O D E -----
	
		var code = 
			"grab";
					
		var world = new GIDGET.World([10,10], [1,8], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("grabbed bird");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToGrab");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	// *******************************************************
	
	parser_missingThingToDrop: function() {

		// ----- G - C O D E -----
	
		var code = 
			"grab bird\n" +
			"drop";
		
		var world = new GIDGET.World([10,10], [1,8], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToDrop");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	
	// *******************************************************
	
	parser_missingPredicate: function() {

		// ----- G - C O D E -----
	
		var code = 
			"analyze bird, if\n";
		
		var world = new GIDGET.World([10,10], [1,8], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingPredicate");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 1, 7, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	
	// *******************************************************
	
	parser_missingConditionalComma: function() {

		// ----- G - C O D E -----
	
		var code = 
			"analyze bird, if it isn't infected\n";
		
		var world = new GIDGET.World([10,10], [1,8], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingConditionalComma");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	
	
	// *******************************************************
	
	//** CHECK AGAIN
	
	parser_missingThingToModify_CHECKAGAIN: function() {

		// ----- G - C O D E -----
	
		var code = 
			"modify";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToModify");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	

	
	
	// *******************************************************
	
	parser_missingThingToAdd: function() {

		// ----- G - C O D E -----
	
		var code = 
			"add";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToAdd");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	
	
	// *******************************************************
	
	parser_missingThingToRemove: function() {

		// ----- G - C O D E -----
	
		var code = 
			"remove";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingThingToRemove");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	
	
	// *******************************************************
	
	parser_missingAndPredicate_CHECKAGAIN: function() {
	
	//** CHECK AGAIN
	
		// ----- G - C O D E -----
	
		var code = 
			"scan cat, goto cat, analyze it, if it is infected and isn't orange, grab it\n" +
			"scan bird, goto it\n" +
			"drop";
		
		var world = new GIDGET.World([10,10], [1,8], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingAndPredicate");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "cat", 1, 9, "orange", [ 'infected', 'orange' ], {});
		new GIDGET.Thing(world, "cat", 1, 8, "orange", [ 'infected', 'blue' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	// *******************************************************
	
	parser_missingTag: function() {

		// ----- G - C O D E -----
	
		var code = 
			"analyze cat, if it isn't, grab it";
		
		var world = new GIDGET.World([10,10], [5,6], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("analyzed cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingTag");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	
	
	// *******************************************************
	
	parser_missingQueryName_CHECKAGAIN: function() {
	
	//** CHECK AGAIN
	
		// ----- G - C O D E -----
	
		var code = 
			"scan bird";
		
		var world = new GIDGET.World([10,10], [1,1], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("one");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingQueryName");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 1, 8, "orange", [], {});
		new GIDGET.Thing(world, "cat", 5, 6, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 9, 6, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
	
	
	
	
	
	// *******************************************************
	
	parser_missingOn: function() {
	
	//** CHECK AGAIN
	
		// ----- G - C O D E -----
	
		var code = 
			"grab bird\n" +
			"grab cat\n" +
			"grab crate";
		
		var world = new GIDGET.World([10,10], [4,7], [], code);
		world.gidget.setEnergy(500);
			
		// ---- G O A L S --------
		
		world.addGoal("gidget on");
		world.addGoal("on cat");

		// ---- T I T L E --------
		
		world.addTitle("Testing: parser_missingOn");		

		// ---- M I S S I O N ----
		
		if (GIDGET.experiment.isControl()) {
			world.addMissionText("sad", "Parser Debugging");
		}		
		else {
			world.addMissionText("sad", "Whee, Parser Debugging!!");
		}
		
		// ----- T H I N G S -----
		
		new GIDGET.Thing(world, "bird", 4, 7, "orange", [], {});
		new GIDGET.Thing(world, "cat", 4, 7, "orange", [ 'infected' ], {});
		new GIDGET.Thing(world, "crate", 4, 7, "Chocolate", [], {});
		
		// -----------------------
								
		return world;
	
	},
												
		
};