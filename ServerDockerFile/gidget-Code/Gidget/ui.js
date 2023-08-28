GIDGET.ui = {

	// The level to initialize on each run.
	level: undefined,

	// Contains all the data for the world being run
	world: undefined,

	messages: "",

	competence: undefined, //adaptive setting

	stepSpeedInMilliseconds: 100,

	images: {},

	stepState: undefined,

	hasImage: function(name, state) {

		if (name == "gidget") {
			return this.images.hasOwnProperty("control." + state) && this.images["control." + state] !== false;
		}
		return this.images.hasOwnProperty(name + "." + state) && this.images[name + "." + state] !== false;

	},

	getImage: function(name, state) {

		// Set Gidget's image for Control Condition
		var label;
		if ((name === "gidget") && (GIDGET.experiment.isControl()))
			label = "control." + state;
		else
			label = name + "." + state;

		// If this has already been checked, return what's there.
		if(this.images.hasOwnProperty(label))
			return this.images[label];

		// For now, mark it as false.
		this.images[label] = false;

		$.ajax({ url: "media/" + label + ".png", context: this,
			success: function(){

	        	// An image has loaded! Create the image, cache it, and update the UI.
				var image = new Image();
				// Only once the image has loaded do we store it and update the image in the database; this is because sometimes local AJAX requests lie.
				// Moreover, we set the callback first, then the source, because sometimes the callback gets called asynchronously before the callback is set
				// when we define the source first.
				image.onload = function () {
		        	GIDGET.ui.images[label] = image;
		        	GIDGET.ui.updateRuntimeUserInterface();
				};
				image.src = "media/" + label + ".png";

	      	},
	      	error: function(XMLHttpRequest, textStatus, errorThrown) {

	      		// Mark this name as not existing
	      		this.images[label] = false;

	      	}
      	});

		return false;

	},

	log: function(message) {

		this.messages = this.messages + "<br>" + message;

		if (GIDGET.experiment.verbose) {
			console.log("[gidget] " + message);
		}

	},

	setCodeToWorldDefault: function() {

		$('#code').html(this.gidgetCodeToHTML(this.world.code));

	},

	getNumberOfLevelsPassed: function() {

		if(localStorage.getItem('levelMetadata') === null)
			return 0;

		var levelMetadata = getLocalStorageObject('levelMetadata');

		var count = 0;
		for(var level in levelMetadata) {
			if(levelMetadata.hasOwnProperty(level)) {
				var data = levelMetadata[level];
				if(data.passed === true)
					count++;
			}
		}
		return count;

	},

	showLevelInfo: function() {
		if (!isDef(this.world.levelNumber))
			this.world.addLevelNumber();
		$('#levelTitle').html("Level " + this.world.levelNumber + ". " + this.world.levelTitle);
	},

	removeSpecialCharacters: function(myString) {

		return myString.replace(/[^a-zA-Z 0-9.();?!]+/g,' ');
	},

	radioEmpty: function(name) {
		if (!isDef($('input[name='+name+']:radio:checked').val())) {return "";}
		else {return $('input[name='+name+']:radio:checked').val();}
	},

	quit: function(message) {

		$('#postSurvey').hide();

		var level = localStorage.getItem('currentLevel');

		$('#quitResults').html("<p>" + message + " Saving your achievements...").show();

		function randomPassword(length) {
		   chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
		   pass = "";
		   for(x = 0; x < length; x++) {
				i = Math.floor(Math.random() * 62);
				pass += chars.charAt(i);
		   }
		   return pass;
		}

		var password = randomPassword(10);



		var systemDetect = {
			init: function () {
				this.browser = this.searchString(this.dataBrowser) || "unknown";
				this.version = this.searchVersion(navigator.userAgent)
					|| this.searchVersion(navigator.appVersion)
					|| "an unknown version";
				this.OS = this.searchString(this.dataOS) || "unknown";
			},
			searchString: function (data) {
				for (var i=0;i<data.length;i++)	{
					var dataString = data[i].string;
					var dataProp = data[i].prop;
					this.versionSearchString = data[i].versionSearch || data[i].identity;
					if (dataString) {
						if (dataString.indexOf(data[i].subString) != -1)
							return data[i].identity;
					}
					else if (dataProp)
						return data[i].identity;
				}
			},
			searchVersion: function (dataString) {
				var index = dataString.indexOf(this.versionSearchString);
				if (index == -1) return;
				return parseFloat(dataString.substring(index+this.versionSearchString.length+1));
			},
			dataBrowser: [
				{
					string: navigator.userAgent,
					subString: "Chrome",
					identity: "Chrome"
				},
				{
					string: navigator.vendor,
					subString: "Apple",
					identity: "Safari",
					versionSearch: "Version"
				},
				{
					prop: window.opera,
					identity: "Opera"
				},
				{
					string: navigator.userAgent,
					subString: "Firefox",
					identity: "Firefox"
				},
				{
					string: navigator.userAgent,
						subString: "MSIE",
					identity: "Explorer",
					versionSearch: "MSIE"
				}
			],
				dataOS : [
				{
					string: navigator.platform,
					subString: "Win",
					identity: "Windows"
				},
				{
					string: navigator.platform,
						subString: "Mac",
					identity: "Mac"
				},
				{
					string: navigator.platform,
					subString: "Linux",
					identity: "Linux"
				}
			]

		};

		systemDetect.init();


		var payload = {
			condition: GIDGET.experiment.condition,
			studentid: $('input[name=stnumber]').val(),
			currentLevel: localStorage.getItem('currentLevel'),
			levelsPassed: this.getNumberOfLevelsPassed(),
			code: password,
			levelMetadata: getLocalStorageObject('levelMetadata'),
			browser: systemDetect.browser,
			os: systemDetect.OS,
			finalTime: (new Date()).getTime(),
			// Extract all of the questionnaire data from the form.
			survey: {
				gender: this.radioEmpty("gender") == "unlisted" ? $('input[name=other_gender]').val() : this.radioEmpty("gender"),
				age: this.removeSpecialCharacters($('input[name=age]').val()),
				//country:  $('select[name=country] option:selected').val(),
				education: $('select[name=education] option:selected').val(),
				experience1: $('input[name=experience1]').attr('checked'),
				experience2: $('input[name=experience2]').attr('checked'),
				experience3: $('input[name=experience3]').attr('checked'),
				experience4: $('input[name=experience4]').attr('checked'),
				experience5: $('input[name=experience5]').attr('checked'),
				experience6: $('input[name=experience6]').attr('checked'),
				enjoyment: this.radioEmpty("enjoyment"),
				recommend: this.radioEmpty("recommend"),
				helpGidget:this.radioEmpty("helpGidget"),
				dialogue: this.removeSpecialCharacters($('textarea[name=freeDialogue]').val()),
				avatar: this.removeSpecialCharacters($('textarea[name=freeAvatar]').val()),
				experience: this.removeSpecialCharacters($('textarea[name=freeExperience]').val()),
				whyQuit: this.removeSpecialCharacters($('textarea[name=freeQuit]').val()),
				whyMore: this.removeSpecialCharacters($('textarea[name=freeMore]').val()),
				CORE01: this.radioEmpty("CORE01"),
CORE02: this.radioEmpty("CORE02"),
CORE03: this.radioEmpty("CORE03"),
CORE04: this.radioEmpty("CORE04"),
CORE05: this.radioEmpty("CORE05"),
CORE06: this.radioEmpty("CORE06"),
CORE07: this.radioEmpty("CORE07"),
CORE08: this.radioEmpty("CORE08"),
CORE09: this.radioEmpty("CORE09"),
CORE10: this.radioEmpty("CORE10"),
CORE11: this.radioEmpty("CORE11"),
CORE12: this.radioEmpty("CORE12"),
CORE13: this.radioEmpty("CORE13"),
CORE14: this.radioEmpty("CORE14"),
CORE15: this.radioEmpty("CORE15"),
CORE16: this.radioEmpty("CORE16"),
CORE17: this.radioEmpty("CORE17"),
CORE18: this.radioEmpty("CORE18"),
CORE19: this.radioEmpty("CORE19"),
CORE20: this.radioEmpty("CORE20"),
CORE21: this.radioEmpty("CORE21"),
CORE22: this.radioEmpty("CORE22"),
PGAME01: this.radioEmpty("PGAME01"),
PGAME02: this.radioEmpty("PGAME02"),
PGAME03: this.radioEmpty("PGAME03"),
PGAME04: this.radioEmpty("PGAME04"),
PGAME05: this.radioEmpty("PGAME05"),
PGAME06: this.radioEmpty("PGAME06"),
PGAME07: this.radioEmpty("PGAME07"),
PGAME08: this.radioEmpty("PGAME08"),
PGAME09: this.radioEmpty("PGAME09"),
PGAME10: this.radioEmpty("PGAME10"),
PGAME11: this.radioEmpty("PGAME11"),
PGAME12: this.radioEmpty("PGAME12"),
PGAME13: this.radioEmpty("PGAME13"),
PGAME14: this.radioEmpty("PGAME14"),
				//doop: this.radioEmpty("doop"),//test survey
			}
		}

		payload = JSON.stringify(payload);

		localStorage.setItem("quit", password);

		$.ajax({
			type: "POST",
			url: "http://sqrlab2.science.uoit.ca:8082/saveData",
      xhrFields: {
         withCredentials: true
      },
			data: payload,
			success: function(msg) {
				GIDGET.ui.disable("Successfully saved your results!");
			},
			error: function(jqXHR, textStatus, errorThrown) {
				GIDGET.ui.disable("Your results could not be saved :(");
			}
		});

	},

	// Hides the whole UI
	disable: function(message) {

		$('#container').hide();

		$('#quitResults').html(
			"<p>" + message + "</p>" +
			//"<p>Your MTurk completion code is: <b>" + localStorage.getItem('quit') + "</b>.</p><p>Please enter it back on the MTurk HIT page.</p>" +
			//"<p>From this point on, the game will be disabled."
			""
		).show();

	},

	// Hides all but the mission and thoughts
	showAllButMission: function(show) {

		var duration = 200;
		var opacity = show ? 0.0 : 1.0;

		$('#instructionsContainer').animate({ 'opacity': opacity }, duration);
		$('#code').animate({ 'opacity': opacity }, duration);
		$('#goals').animate({ 'opacity': opacity}, duration);
		$('#learnerThought').animate({ 'opacity': opacity}, duration);
		$('#rightPanel').animate({ 'opacity': opacity}, duration);

	},

	getCurrentLevel: function() {

		return localStorage.getItem('currentLevel');

	},

	saveCurrentLevelCode: function() {

		var currentCode = this.htmlToGidgetCode($('#code').html());

		var levelData = getLocalStorageObject('levelMetadata');

		// Create an empty object literal to store level versions.
		if(levelData === null)
			levelData = { };

		// Get the list of versions for this level. If there isn't one, make an empty list.
		if(!levelData.hasOwnProperty(this.getCurrentLevel())) {

			levelData[this.getCurrentLevel()] = {
				passed: false,
				startTime: (new Date()).getTime(),
				endTime: undefined,
				versions: [],
				stepLog: [],

				//Adaptive data
				failCount: 0,
				minEnergy: 1000,
				energyUsed: 0,
				solutionLength: undefined,
				totalTime: undefined
			};

		}

		// Add the current version to the list of versions.
		levelData[this.getCurrentLevel()].versions.push({ time: (new Date).getTime(), version: currentCode });

		// Add all steps logged to the store and empty the store in memory.
		levelData[this.getCurrentLevel()].stepLog = levelData[this.getCurrentLevel()].stepLog.concat(this.stepLog);
		this.stepLog = [];

		//ADAPTIVE: Add failCount
		levelData[this.getCurrentLevel()].failCount += this.failCount;
		this.failCount = 0;

		//ADAPTIVE: Add minEnergy
		levelData[this.getCurrentLevel()].minEnergy = this.minEnergy;
		this.minEnergy = 1000;

		//ADAPTIVE: Add energyUsed
		levelData[this.getCurrentLevel()].energyUsed = this.energyUsed;
		this.energyUsed = 0;

		// Stringify the current versions object
		setLocalStorageObject('levelMetadata', levelData);

	},

	gidgetCodeToHTML: function(code) {

		var count = 0;
		var first = false;

		function tokenToHTML(string) {

			var classes = 'sourceToken';

			if(string.match(/scan|say|analyze|goto|ask|to|grab|drop|it|if|is|are|on|avoid/i))
				classes = classes + ' keyword';

			if(first) {
				classes = classes + ' first';
				first = false;
			}

			var html = "<span class='" + classes + "' id='sourceToken" + count + "'>" + string + "</span>";
			count++;
			return html;

		}

		// Convert the given gidget code into marked up HTML amenable for highlighting.
		var lines = code.split(/\r\n|\r|\n/);
		var lineNumber;
		var html = "";
		for(lineNumber = 0; lineNumber < lines.length; lineNumber++) {

			first = true;
			var line = lines[lineNumber];

			var classes = "sourceLine";

			// If this is not Firefox, add indent
			if(!$.browser.mozilla)
				classes = classes + " indent";

			var lineText = "<div class='" + classes + "' id='sourceLine" + lineNumber + "'>";

			// If it was just a line break, keep it
			if(line.length === 0) {

				lineText = lineText + "<br>";

			}
			else {

				var charIndex, char;
				var id = "";
				for(charIndex = 0; charIndex < line.length; charIndex++) {

					char = line.charAt(charIndex);

					// If it's a space, add a space.
					if(char.match(/\s/)) {
						if(id.length > 0)
							lineText = lineText + tokenToHTML(id);
						lineText = lineText + " "; // this needs to be " " instead of &nbsp for text-wrapping purposes.
						id = "";
					}
					// If it's a comma, add the accumulated id if necessary and then add a comma,
					// resetting the accumulated id.
					else if(char === ',') {

						if(id.length > 0)
							lineText = lineText + tokenToHTML(id);
						lineText = lineText + tokenToHTML(",");
						id = "";

					}
					// Otherwise, just accumulate characters for the id.
					else
						id = id + char;

				}

				// If there's text accumulated for the id, generate a token for it.
				if(id.length > 0)
					lineText = lineText + tokenToHTML(id);

			}

			// End the line.
			lineText = lineText + "</div>";

			// Increment the token number to account for the new line
			count++;

			// Add the line to the html.
			html = html + lineText;

		}

		return html;

	},

	htmlToGidgetCode: function(html) {

/* 		console.log("Before\n" + html); */

	    var ce = $("<pre />").html(html);

		// If this is mozilla, opera, or IE, replace non-last child, non-only-child <br> tags with new lines.
		// The reason for this is that <br>s don't actually create a new line when immediately followed by a block.
		// We do this before replacing divs, so we can keep track of which <br>s are only or last children.
		if($.browser.mozilla || $.browser.opera ||$.browser.msie )
  			ce.find("br:not(:last-child,:only-child)").replaceWith("<span>\n</span>");

		// After doing this, replace all divs with new lines.
	    if ($.browser.webkit || $.browser.mozilla) {
	    	// We do this in a loop since divs within divs are eliminated when outer divs are replaced.
	    	// We just keep replacing until there are no more divs.
	    	do {
				var divs = ce.find("div");
				divs.replaceWith(function() { return "\n" + this.innerHTML; });
			} while(divs.length > 0);
		}

		// If this is IE, which inserts <p> tags, replace the <p> with the text followed by a <br>
	    if ($.browser.msie)
			ce.find("p").replaceWith(function() { return this.innerHTML + "<br>"; });

/* 		console.log("After transformation\n" + ce.html()); */

	    var code = ce.text();
	    code = jQuery.trim(code);

/* 		console.log("Extracted, trimmed text\n" + code); */

		return code;

	},

	currentMissionText: undefined,

	// Should only be called once upon starting the level or when the user requests to start over.
	setLevel: function(level) {


		this.level = GIDGET.levels[level];

		localStorage.currentLevel = level;
		this.reset();



		// Initialize the Gidget code with the code provided in the level specification.
		this.setCodeToWorldDefault();

		// Initially disable the reset button
		$('#reset').attr('disabled', true);

		this.currentMissionText = -1;

		this.saveCurrentLevelCode();

		// Set titlebar with level number and title (if there is one)
		this.showLevelInfo();

		this.showAllButMission(true);

		// Show the first mission text.
		this.showNextMissionText();

		// Make the code editable
		$('#code').attr('contentEditable', 'true');

		// Hide all commands in the hidden command list.
		this.hideUnknownCommands();

	},

	performAdaptations: function(competence){

		if (competence == 0){

			this.world.highAdapt();

			//this.world.gidget.setEnergy(this.world.adaptEnergy);
			//example: add extra object
			//new GIDGET.Thing(this.world, "rock", 2, 2, "brown", [], {});

			//example: rat enemy
			/*
			var rat = new GIDGET.Thing(this.world, "rat", 3, 4, "yellow", [], {});
			rat.setCode(
				"say Yum! I want to munch on your tasty wires to take all your energy!\n" +
				"when gidget on rat, set gidget energy 0\n" +
				"scan gidget\n" +
				"goto gidget\n"
			);
			rat.setSpeed(10);
			*/

			//example: battery replacing energy
			/*new GIDGET.Thing(this.world, "battery", 2, 2, "yellow", [],
				{
					energize : new GIDGET.Action([ "beneficiary" ],
						"raise beneficiary energy 10"
					)
				}
			);
			this.world.gidget.setEnergy(this.world.gidget.energy-10);
			*/

			//example: extra goal
			/*new GIDGET.Thing(this.world, "goop", 4, 4, "green", [], {});
			this.world.addGoal("goop on bucket");
			*/

		}
		else if (competence ==2 ){
			console.log("LowAdaptGo");
			this.world.lowAdapt();
			//example: energy boost
			//this.world.gidget.setEnergy(9000);

			//example: add batteries
			/*new GIDGET.Thing(this.world, "battery", 2, 2, "yellow", [],
				{
					energize : new GIDGET.Action([ "beneficiary" ],
						"raise beneficiary energy 10"
					)
				}
			);*/
		}

	},

	hideUnknownCommands: function() {

		$('.cheatsheetItem').show();
		var command;
		for(command in this.world.hiddenCommands) {
			if(this.world.hiddenCommands.hasOwnProperty(command)) {
				$('#cheat-' + command).hide();
			}
		}

	},

	// Checks to see if current code has changed from the original code.
	didCodeChange: function(currentCode) {
		// String comparison doesn't work correctly without removing all whitespace characters
		return (this.world.code.replace(/\s/g, "") != this.htmlToGidgetCode(currentCode).replace(/\s/g, ""));
	},

	showNextMissionText: function() {

		$('*').removeClass('runtimeReference');

		this.currentMissionText++;

		if(this.currentMissionText >= this.world.missionText.length) {

			this.visualizeDecision(new GIDGET.text.Message(this.world.missionText[this.currentMissionText - 1].text));
			this.currentMissionText = undefined;
			this.showExecutionControls();
			this.showAllButMission(false);

		}
		else {

			this.visualizeDecision(new GIDGET.text.Message(this.world.missionText[this.currentMissionText].text + "<div><button onclick='GIDGET.ui.step()' class='align-right'>next...</button></div>", undefined, this.world.missionText[this.currentMissionText].state), false);

		}

	},

	nextLevel: function() {

		// Disable any UI in the learner's thought bubble, so the learner can't do anything yet.
		GIDGET.ui.setThought("<span id='learnerSpeech'></span>", 0, "learner");
		var levelData = getLocalStorageObject('levelMetadata');

		var energy=[];
		var failed=[];
			  var count = 0;
		 for(var level in levelData) {
		  	if(levelData.hasOwnProperty(level)) {
			 	var data = levelData[level];
		 		energy[count] = data.energyUsed.toString();
	 		failed[count] = data.failCount.toString();
				console.log("failed:  "+ data.failCount);
				count++;

			 }
		  }

		 //combining arrays
		
	 var combine = failed.concat(energy); 
		console.log("PlayerData= " + combine);
		//ADAPT run ML script
		//1) combine failCount array + energyUsed array
		//2) pass as parameter to python script
		//3) get return value as this.competence

		// Where  API is
		var url_server = "http://199.212.33.3:8082"
		var endpointURL = "/api/getCompetence"

	
		var playerData = this.failCount + this.energyUsed
		var inputFilename = "data.csv"

		// Pass the parameter to python script (the API endpoint)
		var endpointParameter = "?playerData="+ combine.toString() +"&fileName=" + inputFilename

		console.log(url_server + endpointURL + endpointParameter)
		var fullUrl = url_server + endpointURL + endpointParameter
		console.log("URL: " + fullUrl);

		var xmlHttp = new XMLHttpRequest();
		xmlHttp.open("GET", fullUrl, false);
		xmlHttp.send( null );
		var data= JSON.parse(xmlHttp.responseText);
		console.log("JSON DATA:" + data);
		this.competence = data.solution;
		//if(this.getNumberOfLevelsPassed()>8){
			//$.ajax({
				//type: "GET",
				//url: fullUrl, 
				//success: function (data) {
					//this.competence = data.solution;
				//}
			//});

		//}

		//ADAPT check performance
		/*if (levelData[this.getCurrentLevel()].failCount > 4){
			this.competence = 0;
		}
		else if (levelData[this.getCurrentLevel()].failCount == 0){
			this.competence = 2;
		}*/




		// Remember that this level was passed, what time, and the final code.
		this.saveCurrentLevelCode();
		//var levelData = getLocalStorageObject('levelMetadata');
		levelData[this.getCurrentLevel()].passed = true;
		levelData[this.getCurrentLevel()].endTime = (new Date()).getTime();
		setLocalStorageObject('levelMetadata', levelData);

		// Now find the next level.
		var found = false;
		var nextLevel = undefined;
		for(var level in GIDGET.levels) {
			if(GIDGET.levels.hasOwnProperty(level)) {
				if(found) {
					nextLevel = level;
					break;
				}
				else if(level === localStorage.currentLevel) {
					found = true;
				}
			}
		}



		if(isDef(nextLevel)) {

			this.setLevel(nextLevel);

		}
		else {
			$('#container').fadeTo(1000, 0.0);
			$('#finishedLevels').show();
			//this.quit("Congratulations! You beat all of the levels.");

		}

	},

	// Restores the defaults for the current level.
	reset: function() {

		this.messages = "";

		// Restore the world to its defaults.
		this.world = this.level.call();


		//adaptive condition
		this.energyUsed = 0;
		if (GIDGET.experiment.adapt && this.world.levelNumber > 9) {
		//	this.world.gidget.setEnergy(9000);
			this.performAdaptations(this.competence);
		}

		$('#goals').empty();

		// Add the goals text.
		var i;
		var table = "<table>";
		for(i = 0; i < this.world.goals.length; i++)
			table = table + "<tr><td><div class='goal' id='goal" + i + "'>" + this.gidgetCodeToHTML(this.world.goals[i]) + "</div></td><td><span class='success'>&#x2714;</span><span class='failure'>&#x2716;</span></td></tr>";

		$('#goals').html(table);

		$('.success').hide();
		$('.failure').hide();

		$('#goals').removeClass('runtimeReference');

		this.done();



		this.drawGrid();

		this.updateRuntimeUserInterface();



	},

	done: function() {

		this.highlightToken(undefined);

	},

	start: function() {

		// This creates a new world, reset to its defaults.
		this.reset();

		$('#goal').css('backgroundColor', 'rgb(42,33,28)');

		$('#code').attr('contentEditable', 'false');

		// Start the world, but give Gidget the users code.
		this.world.start(this.htmlToGidgetCode($('#code').html()));

		this.updateRuntimeUserInterface();

	},

	showExecutionControls: function() {

		var message;
		if (GIDGET.experiment.condition)
			message = "Tell Gidget to execute:";
		else
			message = "Gidget, please execute...";

		GIDGET.ui.setThought("<span id='learnerSpeech'>"+message+"</span> <br>\n" +
			"<button id='step' onclick='hideToolTip(); GIDGET.ui.stepOnce();' title='Ask Gidget to execute one step of the instructions - there may be multiple steps per line - this button may need to be pressed multiple times to go through the instructions and evaluate the goals. This also toggles the command helper sheet when errors are detected.'>one<br />step</button>\n" +
			"<button id='line' onclick='hideToolTip(); GIDGET.ui.runToNextLine();' title='Ask Gidget to execute one whole line of the instructions - this button may need to be pressed multiple times to go through the instructions and evaluate the goals. This also toggles the command helper sheet when errors are detected.'>one<br />line</button>\n"+
			"<button id='play' onclick='hideToolTip(); GIDGET.ui.playToEnd();' title='Ask Gidget to execute all the instruction and check the goals step-by-step. This button does not stop for any errors.'>all<br />steps</button>\n" +
			"<button id='end'  onclick='hideToolTip(); GIDGET.ui.runToEnd();' title='Ask Gidget to execute all the instructions and check the goals in one step, showing only the final output. This button does not stop for any errors.'>to<br />end</button>\n",
			0, "learner");

	},

	retryLevel: function() {

		GIDGET.ui.showExecutionControls();
		GIDGET.ui.currentMissionText = undefined;
		$('#code').attr('contentEditable', 'true');
		GIDGET.ui.reset();

	},

	showLevelControls: function(succeeded) {
		var message;
		if (GIDGET.experiment.isControl())
			message = "Tell Gidget to:";
		else
			message = "Gidget, let's...";

		if (!succeeded) {
			GIDGET.ui.setThought(
				"<span id='learnerSpeech'>"+message+"</span> <br>" +
				"<button onclick='GIDGET.ui.retryLevel();'>retry this mission!</button><br>",
				0, "learner");
		} else {
			GIDGET.ui.setThought(
				"<span id='learnerSpeech'>"+message+"</span> <br>" +
				"<button onclick='GIDGET.ui.nextLevel()'>start the next mission!</button><br>" +
				"<button onclick='GIDGET.ui.retryLevel()'>redo this mission!</button><br>",
				0, "learner");
		}

	},

	modifyCommStylesForControl: function() {
		$('#learnerThought').css('margin-top', '.5em');
		$('.bubbleText').css('font-family', '\"Courier New\", Courier, monospace');
		$('#code').css({'border-radius': '.25em .25em 0em 0em', '-moz-border-radius': '.25em .25em 0em 0em'})
		$('#goals').css({'border-radius': '0em 0em .25em .25em', '-moz-border-radius': '0em 0em .25em .25em'})
		$('#memory').css({'border-radius': '.25em', '-moz-border-radius': '.25em'})

		$("body").append("<style type='text/css'>.bubbleText button{font-family: \"Courier New\", Courier, monospace;}</style>");
	},

	modifyCommStylesForExperimental: function() {
		$('.bubbleText').css('font-family', 'Verdana, Arial, Helvetica, sans-serif');
		$('#code').css({'border-radius': '1em .5em 0 0', '-moz-border-radius': '1em .5em 0 0'})
		$('#goals').css({'border-radius': '0 0 1em .5em', '-moz-border-radius': '0 0 1em .5em'})
		$('#memory').css({'border-radius': '1em .5em 1em .5em', '-moz-border-radius': '1em .5em 1em .5em'})

		$("body").append("<style type='text/css'>.bubbleText button{font-family: Verdana, Arial, Helvetica, sans-serif;}</style>");
	},

	modifyCommStyles: function() {
		if (GIDGET.experiment.isControl())
			this.modifyCommStylesForControl();
		else
			this.modifyCommStylesForExperimental();
	},


	createLearnerBubble: function(message) {

		var image;
		switch (GIDGET.experiment.condition) {
			case "control":
		 		image = "satellite";
		 		break;
		 	case "female":
		 		image = "female";
		 		break;
		 	case "male":
		 		image = "male";
		 		break;
		 	default:
		 		image = "experimental";
		 		break;
		}

		if (GIDGET.experiment.isControl()){
			return "<div class='thoughtBubbleControl'><table class='thoughtTable'><tr><td><img src='media/" + image + ".default.png' class='thing' title='This is you.' style='padding: 0 .15 0 .15em;' /></td><td style='width: 100%;'>" + message + "</td></tr></table></div>";
		}
		else {
			return "<table class='thoughtTable thoughtTableLearner'><tr><td><img src='media/" + image + ".default.png' class='thing' title='This is you!' style='display: block;' /><img src='media/speechTailLearner.default.png' style='position: relative; right: -1.85em; top: -2em;' /></td><td class='thoughtBubbleCommunication' style='padding: 1em 0 1em 1em;'>" + message + "</td></tr></table>";
		}

	},


	createGidgetBubble: function(message) {

		var gidgetImg;
		if (GIDGET.experiment.isControl())
			gidgetImg = "media/control.";
		else
			gidgetImg = "media/gidget.";

		if (GIDGET.experiment.isControl()){
			return "<div class='thoughtBubbleControl' style='width: 18em;'><table class='thoughtTable'><tr><td><img src='" + gidgetImg + this.world.gidget.runtime.state + ".png' class='thing' title='This is your communication window with Gidget' style='padding: 0 1em 0 .5em;' /></td><td><span id='gidgetSpeech'>" + message + "</span></td></tr></table></div>";
		}
		else {
			return "<table class='thoughtTable thoughtTableGidget'><tr><td class='thoughtBubbleCommunication'><span id='gidgetSpeech'>" + message + "</span></td><td><img src='" + gidgetImg +  this.world.gidget.runtime.state + ".png' class='thing' title='This is Gidget communicating with you!' style='display: block;' /><img src='media/speechTailGidget.default.png' style='position: relative; left: -1.6em; top: -2.2em;' /></td></tr></table>";
		}

	},


	setThought: function(message, time, target) {

		var html;
		var delay = isDef(time) ? time : 0;
		var who = isDef(target) ? target : "gidget";

		if (who === "gidget")
			html = this.createGidgetBubble(message);
		else
			html = this.createLearnerBubble(message);

		if(delay === 0) {
			$('#'+who+"Thought").html(html);
		}
		else {

			$('#'+who+"Thought").animate({
				opacity: 0.0
			}, delay,
				function() {
					$('#'+who+"Thought").html(html);
					$('#'+who+"Thought").animate({ opacity: 1.0 }, delay);
				}
			);

		}

	},


	// Used to temporarily store the list of commands logged for this level.
	// These are saved when the levelMetadata is saved.
	stepLog: [],

	failCount: 0,

	minEnergy: 1000,

	energyUsed: 0,

	currentExecutionMode: undefined,

	logStep: function(kind) {

		this.currentExecutionMode = kind;

		// Save the kind of command, when it happened, and for which level.
		this.stepLog.push({
			kind: kind,
			time: (new Date()).getTime()
		});

	},

	stepOnce: function() {
		this.logStep("step");
		GIDGET.ui.step(false, true);
		this.currentExecutionMode = undefined;
	},

	// Just a helper function for playToEnd() to call.
	stepContinue: function() {

		GIDGET.ui.step(true, false);

	},

	// Executes, but does not animate, each instruction
	runToEnd: function() {

		GIDGET.ui.media.disableSounds = true;

		this.logStep("end");

		this.enableExecutionButtons(false);

		// Start the world.
		if(!this.world.isExecuting())
			this.start();

		while(this.world.isExecuting())
			GIDGET.ui.step(false, false);

		this.executeGoals();

		GIDGET.ui.media.disableSounds = false;

		while(this.goalNumberBeingExecuted <= this.world.goals.length)
			GIDGET.ui.step(false, false);

		this.enableExecutionButtons(true);

		this.currentExecutionMode = undefined;

	},

	playToEnd: function() {

		this.logStep("play");

		this.enableExecutionButtons(false);

		// Call step repeatedly until done.
		setTimeout(GIDGET.ui.stepContinue, this.stepSpeedInMilliseconds);

	},

	enableExecutionButtons: function(enabled) {

		var setting = !enabled;

		$('#play').attr('disabled', setting);
		$('#step').attr('disabled', setting);
		$('#line').attr('disabled', setting);
		$('#end').attr('disabled', setting);

	},

	// Runs to the next line, starting execution if it hasn't been started yet.
	runToNextLine: function() {

		this.logStep("line");

		this.enableExecutionButtons(false);

		// If there is no current line, then run until there is one, running through all of the mission text.
		if(isDef(this.currentMissionText)) {

			while(isDef(this.currentMissionText))
				GIDGET.ui.step(false, false);

			// Run the first step
			GIDGET.ui.step(false, false);

		}
		// If we're executing goals, step to the next goal
		else if(isDef(this.goalNumberBeingExecuted)) {

			var goal = this.goalNumberBeingExecuted;
			while(this.goalNumberBeingExecuted === goal)
				GIDGET.ui.step(false, false);

			// Show the results
			if(this.goalNumberBeingExecuted >= this.world.goals.length)
				GIDGET.ui.step(false, false);

		}
		// Otherwise, step to the next line
		else {

			// Start the world if it's not started
			if(!this.world.isExecuting()) {
				GIDGET.ui.step(false, false);
			}
			else {

				var currentLine = this.world.gidget.runtime.getCurrentLine();
				while(this.world.isExecuting() && this.world.gidget.runtime.getCurrentLine() === currentLine)
					GIDGET.ui.step(false, false);

			}

		}

		this.enableExecutionButtons(true);

	},

	// This variable is used throughout the UI to animate transitions between steps.
	percentRemaining: 100,

	// This variable is used to indicate whether scan is being animated, so it can be drawn.
	animatingScanned: undefined,

	// A hash table of lists of decisions, indexed by thing. These are the decisions
	// remaining to be executed before moving on to the next step.
	decisionsRemaining: [],

	// Used by step() to remember which goal is being executed.
	goalNumberBeingExecuted: undefined,
	allGoalsAchieved: undefined,

	// This steps the world one step or executes the goal if the world is done executing.
	step: function(play, animate) {

		// Hide the cheatsheet
		GIDGET.ui.toggleCheatsheet(false);

		// Update communication bubble styles for condition
		//GIDGET.ui.modifyCommStyles();

		// Remove highlighting
		this.hideUnknownCommands();
		$('#cheatsheet').removeClass('runtimeReference');
		$('#cheatsheet .cheatsheetItem').removeClass('runtimeReference');

		setTimeout(GIDGET.ui.animate);

		// Used to iterate through the decisions generated by executing the steps for each thing.
		var index;

		// If the current mission text index is not undefined, we're still displaying mission text.
		if(isDef(this.currentMissionText)) {

			// Show the text and advance to the next text, if there is some.
			this.showNextMissionText();

		}
		// If we're not displaying mission text, but we are executing a goal, step through the goal
		else if(isDef(this.goalNumberBeingExecuted)) {

			// We just finished executing the normal code, but have yet to start the first goal.
			if(this.goalNumberBeingExecuted < 0) {

				// Initialize the current goal.
				this.goalNumberBeingExecuted = 0;

				// Start by assuming all goals are achieved; when one is not, mark it false.
				this.allGoalsAchieved = true;

				// Start the initial goal.
				this.world.gidget.runtime.start(this.world.goals[this.goalNumberBeingExecuted], true, {});

				// Step once.
				this.step();

			}
			// We've tested all goals. Time for final feedback.
			else if(this.goalNumberBeingExecuted >= this.world.goals.length) {

				// Stop executing goals!
				this.goalNumberBeingExecuted = undefined;

				// Reset the UI
				this.done();

				// Erase the token highlighting.
				this.highlightToken(undefined);

				var succeeded = undefined;

				if(this.allGoalsAchieved === true) {

					succeeded = true;
					this.visualizeDecision(GIDGET.text.goal_finalSuccess(), true);

					GIDGET.ui.media.playSound("goal_finalSuccess");

					this.rememberLevelPassed();

				}
				else {

					succeeded = false;
					this.failCount++;
					this.visualizeDecision(GIDGET.text.goal_finalFailure(), true);

					GIDGET.ui.media.playSound("goal_finalFailure");
				}

				this.enableExecutionButtons(true);

				this.showLevelControls(succeeded);

				// We're done, so reset the execution mode.
				this.currentExecutionMode = undefined;

				// In case we're playing, return without invoking another step.
				return;

			}
			// If this goal isn't done executing yet, step it.
			else if(this.world.gidget.runtime.isExecuting()) {

				var decisions = this.world.gidget.runtime.step();
				for(index = 0; index < decisions.length; index++) {

					decisions[index].execute();

					// Update the runtime UI
					this.updateRuntimeUserInterface(decisions[index].action);

					// Highlight the runtime UI to show what changed.
					this.visualizeDecision(decisions[index].thought, animate);

				}

			}
			// If we just finished executing this goal, provide feedback and move on to the next goal
			else {

				// Are there results? If so, the goal was achieved.
				// Put a check mark.
				if(this.world.gidget.runtime.hasRecentResults()) {

					$('#goals .success:eq(' + this.goalNumberBeingExecuted + ')').show();
					this.visualizeDecision(GIDGET.text.goal_checkSuccess(), true);

					GIDGET.ui.media.playSound("goal_checkSuccess");

				}
				// If there aren't results, the goal wasn't achieved.
				// Put an x mark.
				else {

					$('#goals .failure:eq(' + this.goalNumberBeingExecuted + ')').show();
					this.allGoalsAchieved = false;
					this.visualizeDecision(GIDGET.text.goal_checkFailure(), true);

					GIDGET.ui.media.playSound("goal_checkFailure");
				}

				// Erase the token highlighting.
				this.highlightToken(undefined);

				// Move on to the next goal.
				this.goalNumberBeingExecuted++;

				// Start the next goal, if there is one.
				if(this.goalNumberBeingExecuted < this.world.goals.length)
					this.world.gidget.runtime.start(this.world.goals[this.goalNumberBeingExecuted], true, {});

			}

		}
		// Otherwise, we're just executing the normal code.
		else {

			// Start the world if it's not executing.
			if(!this.world.isExecuting())
				this.start();

			// If the world is still executing, execute each thing's next decision.
			if(this.world.isExecuting()) {

				function countDecisionsRemaining(world) {

					// How many decisions are left to execute across all things in the world?
					var decisionCount = 0;
					for(index = 0; index < world.decisionsRemaining.length; index++)
						decisionCount += world.decisionsRemaining[index].length;
					return decisionCount;

				}

				// If there are none left, get some more by stepping the world once.
				while(this.world.isExecuting() && countDecisionsRemaining(this) === 0)
					this.decisionsRemaining = this.world.step();

				// Reset the animation loop.
				GIDGET.ui.world.resetThingDeltas();
				this.percentRemaining = 100;

				var somethingSaid = false;

				// Explain each active thing's next decision before executing it.
				for(index = 0; index < this.decisionsRemaining.length; index++) {

					// Is there at least one decision? Get the next one and execute it.
					if(this.decisionsRemaining[index].length > 0) {

						var decision = this.decisionsRemaining[index].shift();
						var runtime = decision.runtime;

						// Execute the decision's action, if there is one.
						decision.execute();

						// If this is Gidget, visualize the decision
						if(runtime.thing.name === 'gidget') {

							// Update the runtime UI
							this.updateRuntimeUserInterface(decision.action);

							// Highlight the runtime UI to show what changed.
							this.visualizeDecision(decision.thought, animate);

						}
						// Handle non-gidget visualization.
						else {

							// Play the sound if there is one.
							if(isDef(decision.thought) && isDef(decision.thought.sound) && isDef(decision.step) && decision.step.ast.type == 'modify') {

								GIDGET.ui.media.playSound(decision.thought.sound);

							}

						}

						if (GIDGET.experiment.verbose) {
							console.log("[" + runtime.thing.name + ']' + decision.thought.text);
						}

						// If this is a say command, add a speech bubble
						if ((decision.action !== undefined) && (decision.action.kind === 'Say')) {

							// Add the message to the thought bubble.
							$('#thingThought').html(decision.action.message);

							// Make it visible, so we can size it.
							$('#thingThought').css('visibility', 'visible');

							// Choose a location for it that minimizes how much it obscures other Gidget UI.

							// This gets the thing's row position, so the thought appears below the thing.
							var top = runtime.thing.row * (this.getCellSize() + 1) + $('#thingThought').outerHeight() + $('#grid').position().top;

							// This gets the thing's horizontal center.
							var left = 1.5 * runtime.thing.column * this.getCellSize() + $('#grid').position().left - $('#thingThought').outerWidth() / 2;

							// Is the left past the left margin of the world?
							if(left < $('#grid').position().left - this.getCellSize()) left = $('#grid').position().left - this.getCellSize();

							// If the bottom is past the bottom margin of the window, move it above the thing
							if(top + $('#thingThought').outerHeight() > $('#grid').position().top + $('#grid').outerHeight())
								top = $('#grid').position().top + (runtime.thing.row) * (this.getCellSize() + 1) - $('#thingThought').outerHeight();

							// Place the location of it.
							$('#thingThought').css('top', "" + top + "px");
							$('#thingThought').css('left', "" + left + "px");


						}
						// In all other cases, hide the thought bubble.
						else {
							$('#thingThought').css('visibility', 'hidden');
						}

					}

				}

			}

			// If the main script is no longer executing, start executing the goal.
			if(!this.world.isExecuting()) {

				this.executeGoals();

			}

		}

		// If we're playing, invoke another step.
		if(play === true && this.world.gidget.energy > 0)
			setTimeout(GIDGET.ui.stepContinue, GIDGET.ui.stepSpeedInMilliseconds);

	},

	executeGoals: function() {

		var runtime = this.world.gidget.runtime;
		var i, j, goalIndex;
		var allGoalsAchieved;

		// Gidget didn't make it to the end of his program with any energy left, so we
		// don't test the goals.
		if(this.world.gidget.energy <= 0) {

			this.showLevelControls(false);
			this.visualizeDecision(GIDGET.text.noEnergy(), true);

			this.failCount++;
		}
		// Execute each goal.
		else {

			// Remove the highlighting on the normal code
			this.highlightToken(undefined);

			this.goalNumberBeingExecuted = -1;

			this.visualizeDecision(GIDGET.text.aboutToStartGoals(), true);

		}

	},

	// This takes a Thing's decision and converts the spans of text and references
	// into text and user interface highlights. Currently, this is only written for Gidget.
	visualizeDecision: function(message, animate) {

		// Go through the decisions references and highlight the desired references,
		// constructing the html to display in the thought bubble.
		var thought = "";

		if(isDef(message) && isDef(message.text)) {
			var spans = this.parseThought(message.text);
			var span;
			var text;
			var reference, index;

			for(span = 0; span < spans.length; span++) {

				text = spans[span].text;
				reference = spans[span].reference;
				index = parseInt(spans[span].index);
				index = isNaN(index) ? undefined : index;

				// If there's a reference, make a span to represent it.
				if(isDef(reference)) {

					thought = thought + "<span class='runtimeReference'>" + text + "</span>";

					if(reference === 'instructions') {
						$('#instructionsContainer').animate({'opacity': 1.0}, 200);
						$('#code').animate({'opacity': 1.0}, 200);
						$('#code').addClass('runtimeReference');
					}
					else if(reference === 'memory') {
						$('#rightPanel').animate({'opacity': 1.0 }, 200);
						$('#memory').addClass('runtimeReference');
					}
					else if(reference === 'goals') {
						$('#goals').animate({'opacity': 1.0 }, 200);
						$('#goals').addClass('runtimeReference');
					}
					else if(reference === 'controls') {
						this.showExecutionControls();
						$('#learnerThought').show().animate({'opacity': 1.0}, 200);
						if (GIDGET.experiment.isControl())
							$('#learnerThought .thoughtBubbleControl').addClass('runtimeReference');
						else
							$('#learnerThought .thoughtBubbleCommunication').addClass('runtimeReference');

					}

				}
				// Otherwise, just concatenate the text.
				else {

					thought = thought + text;

				}

			}
		}

		// If there's a message
		if(isDef(message)) {

			if(isDef(message.emotion)) {

				this.world.gidget.runtime.state = message.emotion;

			}

			// Play the sound if there is one.
			if(isDef(message.sound)) {

				GIDGET.ui.media.playSound(message.sound);

			}

			// Call its custom behavior, if defined.
			if(isDef(message.functionToCall)) {

				message.functionToCall.call();

			}

		}

		this.setThought(thought, animate === true ? 50 : 0);

		// Only for debugging purposes...
		// this.log(thought);

	},

	rememberLevelPassed: function() {

		// Remember that the user passed the level and when
		var levelData = getLocalStorageObject('levelMetadata');
		levelData[this.getCurrentLevel()].passed = true;
		levelData[this.getCurrentLevel()].endTime = (new Date()).getTime();
		var codeLineCount = levelData[this.getCurrentLevel()].versions[levelData[this.getCurrentLevel()].versions.length - 1].version.split("\n").length;
		levelData[this.getCurrentLevel()].solutionLength = codeLineCount;
		levelData[this.getCurrentLevel()].totalTime = levelData[this.getCurrentLevel()].endTime - levelData[this.getCurrentLevel()].startTime;
		levelData[this.getCurrentLevel()].minEnergy = this.minEnergy;
		levelData[this.getCurrentLevel()].energyUsed = this.energyUsed;
		setLocalStorageObject('levelMetadata', levelData);

		this.updateBonus();

	},

	// If show is defined, either hides or shows. Otherwise, toggles.
	toggleCheatsheet: function(show) {

		// If the world is executing, but not one step at a time, don't show anything.
		if(isDef(this.currentExecutionMode) && this.currentExecutionMode !== 'step')
			return;

		// Make sure the cheatsheet is always top aligned with the code editor.
		$('#cheatsheet').css('top', ($('#code').offset().top + 20) + 'px');
		$('#cheatsheet').css('left', "" + ($('#code').offset().left + $('#code').outerWidth() - 2) + "px");

		var hidden = $('#cheatsheet').css('display') === 'none';

		// If the caller wants to explicitly show or hide the cheatsheet, we already know what action to take.
		// Otherwise, we have to get the current state.
		if(!isDef(show))
			show = hidden;

		// If we're showing, set display to visible and slide out to the right
		if(show && hidden) {

			$('#cheatsheet').animate({'width': '22em'}, 200);
			$('#toggleCheatsheet').text("?"); // hide instructions key

		}
		else if(!hidden) {

/* 			$('#cheatsheet').animate({ 'left': 0 }, 200, function() { $('#cheatsheet').hide(); } ); */
			$('#cheatsheet').animate({'width': '0em'}, 200, function() { $('#cheatsheet').hide(); });

			$('#toggleCheatsheet').text("?"); // show instructions key

		}

	},

	// Only works when stepping one instruction at a time; stepping by line, playing, or running to end don't show the cheatsheet.
	highlightCommand: function(command) {

		GIDGET.ui.toggleCheatsheet(true);
		$('.cheatsheetItem').hide();
		$('#cheat-' + command).show().addClass('runtimeReference');

	},

	updateBonus: function() {

		levelsPassed = this.getNumberOfLevelsPassed();
	//	$('#bonus').html("$" + (levelsPassed * GIDGET.experiment.bonusPerLevel).toFixed(2));
		$('#passed').html(levelsPassed);

	},

	parseThought: function(message) {

		var spans = [];

		// Parse markup of this format: $OBJECT[INDEX](text)
		// Keep finding the index of the next $ until there are no more.
		var indexOfDollar = message.indexOf('$');
		var left, right = message, reference, index, text;
		while(indexOfDollar >= 0) {

			// Split into left and right, skipping the dollar sign
			left = right.substring(0, indexOfDollar);
			right = right.substring(indexOfDollar + 1, message.length);

			// Add a span for the text on the left.
			spans.push({ reference: undefined, index: undefined, text: left });

			// Parse the right part until reaching the )
			reference = "";
			while(right.length > 0 && right.charAt(0).match(/^[a-zA-Z]$/)) {

				reference = reference + right.charAt(0);
				right = right.substring(1, right.length);

			}

			// Is there an index?
			index = "";
			if(right.charAt(0) === '@') {

				right = right.substring(1, right.length);
				while(right.length > 0 && right.charAt(0).match(/^[0-9]$/)) {

					index = index + right.charAt(0);
					right = right.substring(1, right.length);

				}

			}

			// Next there should be a left parenthesis, indicating what text should be highlighted to refer to the referent.
			if(right.length > 0)
				right = right.substring(1, right.length);

			text = "";
			while(right.length > 0 && right.charAt(0) !== ')') {

				text = text + right.charAt(0);
				right = right.substring(1, right.length);

			}

			// Eat the closing parenthesis.
			if(right.length > 0)
				right = right.substring(1, right.length);

			// Add a span to represent the reference.
			spans.push({ reference: reference, index: index, text: text });

			// Find the next dollar sign.
			indexOfDollar = right.indexOf('$');

		}

		// Add a span for the remaining text.
		spans.push({ reference: undefined, index: undefined, text: right });

		return spans;

	},

	updateRuntimeUserInterface: function(action) {

		this.referencedThings = [];

		function thingToHTML(thing) {

			var thingImage, name = thing.name;

			var thingBox = $('<div class="thingBox" title="This is a '+ name +'."></div>');
			// Fix a gidget scanned image issue
			if (name === 'gidget'){
				if (GIDGET.experiment.isControl())
					thingImage = $("<img src='media/control.default.png' class='thing' />");
				else
					thingImage = $("<img src='media/gidget.default.png' class='thing' />");
			}
			else
				thingImage = $("<img src='media/" + (GIDGET.ui.hasImage(name, thing.runtime.state) ? name : "unknown") + "." + thing.runtime.state + ".png' class='thing' />");
			var thingLabel = $("<div class='thingLabel'>" + name + "</div>");
			thingBox.data('thing', thing);
			thingBox.append(thingLabel);
			thingBox.append(thingImage);

			return thingBox;

		}

		function thingListToHTML(array) {

			var thingList = $("<div class='thingList'></div>");
			thingList.append($("<span class='bracket'>[</span>"));

			for(var i = 0; i < array.length; i++)
				thingList.append(thingToHTML(array[i]));

			thingList.append($("<span class='bracket'>]</span>"));

			thingList.append($("<br style='height:0em;clear:both'>"));

			return thingList;

		}

		function listOfThingListsToHTML(listOfLists) {

			if(listOfLists.length === 0) return $("empty");

			var list = $("<div></div>");
			for(var i = 0; i < listOfLists.length; i++) {

				var thingList = thingListToHTML(listOfLists[i]);

				if(listOfLists[i].hasOwnProperty('query') && isDef(listOfLists[i].query)) {

					if(listOfLists[i].query.hasOwnProperty('ast') && listOfLists[i].query.hasOwnProperty('parentAST') && listOfLists[i].query.parentAST.hasOwnProperty('keyword') && isDef(listOfLists[i].query.parentAST.keyword)) {
						var html = GIDGET.ui.gidgetCodeToHTML(listOfLists[i].query.parentAST.keyword.text + " " + listOfLists[i].query.ast.name.text);
						thingList.prepend($('<div>from </div>').append($(html).addClass('codeContainer').css('display', 'inline')));
					}

				}

				list.append(thingList);

			}

			return list;

		}

		// If there's an action specified, only update the particular part of the UI.
		if(isDef(action)) {

			var duration = 500;

			this.animatingScanned = undefined;

			// Animate draw grid on the given thing
			// If the action is pop results, animate the first thing list results away
			if(action.kind === 'PopResults') {
				$('#results .thingList:first .thingBox').
					addClass('runtimeReference').
					hide(duration, function() { GIDGET.ui.updateRuntimeUserInterface(); });
			}
			// Hide just one result
			else if(action.kind === 'PopResult') {
				$('#results .thingList:eq(0) .thingBox:eq(0)').
					addClass('runtimeReference').
					hide(duration, function() { GIDGET.ui.updateRuntimeUserInterface(); });

			}
			else if(action.kind === 'PushResults') {

				// First update the results in the runtime UI, then highlight them.
				GIDGET.ui.updateRuntimeUserInterface();

				if(isDef(action.query) && action.query.name.text === 'it') {
					$('#focused .thingBox:eq(0)').
						addClass('runtimeReference');
				}

				$('#results .thingList:eq(0) .thingBox').
					addClass('runtimeReference').
					hide().show(duration);

				// Highlight all of the things pushed.
				this.referencedThings = [].concat(this.world.gidget.runtime.peekResults());

			}
			// If its scanned, update the UI to include the new element, immediately hide it, and then animate it in.
			else if(action.kind === 'PushScanned') {
				GIDGET.ui.updateRuntimeUserInterface();
				$('#scanned .thingBox:eq(0)').
					addClass('runtimeReference').
					hide().show(duration);

				// Highlight what was scanned
				this.referencedThings = [ this.world.gidget.runtime.scanned[0] ];

				this.animatingScanned = this.world.gidget.runtime.scanned[0];

			}
			// If its scanned, update the UI to include the new element, immediately hide it, and then animate it in.
			else if(action.kind === 'PushAnalyzed') {
				GIDGET.ui.updateRuntimeUserInterface();
				$('#analyzed .thingBox:first').
					addClass('runtimeReference').
					hide().show(duration);

				// Highlight what was analyzed
				this.referencedThings = [ this.world.gidget.runtime.analyzed[0] ];

			}
			// If its scanned, update the UI to include the new element, immediately hide it, and then animate it in.
			else if(action.kind === 'PushGrabbed') {
				GIDGET.ui.updateRuntimeUserInterface();
				$('#grabbed .thingBox:first').
					addClass('runtimeReference').
					hide().show(duration);

				// Highlight what was analyzed
				this.referencedThings = [ this.world.gidget.runtime.grabbed[0] ];

			}
			// If its scanned, update the UI to include the new element, immediately hide it, and then animate it in.
			else if(action.kind === 'PopGrabbed') {
				$('#grabbed .thingBox:eq(' + action.index + ')').
					addClass('runtimeReference').
					hide(duration, function() { GIDGET.ui.updateRuntimeUserInterface(); });
			}
			// If its scanned, update the UI to include the new element, immediately hide it, and then animate it in.
			else if(action.kind === 'PushFocus') {
				GIDGET.ui.updateRuntimeUserInterface();
				$('#focused .thingBox:first').
					addClass('runtimeReference').hide().show(duration);

				// Highlight what was focused
				this.referencedThings = [ this.world.gidget.runtime.focused[0] ];

			}
			// If its scanned, update the UI to include the new element, immediately hide it, and then animate it in.
			else if(action.kind === 'PopFocus') {
				$('#focused .thingBox:eq(0)').
					addClass('runtimeReference').
					hide(duration, function() { GIDGET.ui.updateRuntimeUserInterface(); });
			}
			// If its scanned, update the UI to include the new element, immediately hide it, and then animate it in.
			else if(action.kind === 'IncrementPC') {


			}
			// If its scanned, update the UI to include the new element, immediately hide it, and then animate it in.
			else if(action.kind === 'Move') {

				GIDGET.ui.updateRuntimeUserInterface();
				$('#results .thingList:eq(0) .thingBox:eq(0)').
					addClass('runtimeReference').
					show(duration);

				// Highlight what we're moving to.
				this.referencedThings = [ this.world.gidget.runtime.resultsStack[0][0] ];

			}
			else
				this.updateRuntimeUserInterface();

		}
		else {

			var runtime = this.world.gidget.runtime;

			$('#results').empty();
			$('#focused').empty();
			$('#scanned').empty();
			$('#analyzed').empty();
			$('#grabbed').empty();

			$('#results').append(listOfThingListsToHTML(runtime.resultsStack));
			if(runtime.focused.length > 0)
				$('#focused').append(thingToHTML(runtime.focused[0]));//thingListToHTML(runtime.focused));
			$('#scanned').append(thingListToHTML(runtime.scanned));
			$('#analyzed').append(thingListToHTML(runtime.analyzed));
			$('#grabbed').append(thingListToHTML(runtime.grabbed));

			GIDGET.ui.drawGrid();

			if(isDef(runtime.lastPC)) {

				var token = runtime.steps[runtime.lastPC].representativeToken;

				// If we're executing a goal, highlight it's token
				if(isDef(this.goalNumberBeingExecuted)) {
					if(isDef(token))
						this.highlightToken(token, true);
				}
				else {
					if(isDef(token))
						this.highlightToken(token, false);
				}
			}

			$('#energy').html(runtime.thing.energy + " units");

			if(runtime.thing.energy <= 0)
				$('#energy').css('color', 'red');
			else if(runtime.thing.energy < 20)
				$('#energy').css('color', 'yellow');
			else
				$('#energy').css('color', 'green'); //bright green: #00FF00

			// Add mouse over events for things
			$('.thingBox').mouseenter(function() {
				$(this).addClass('selection').css('border', '4px solid rgb(0,255,0)');
				GIDGET.ui.highlightHoveredThing($(this).data('thing'));
			});
			$('.thingBox').mouseleave(function() {
				$(this).removeClass('selection').css('border', '');
				GIDGET.ui.unhighlightHoveredThing();
			});

		}

	},

	hoveredThing: undefined,

	// Things referenced in thoughts that should be highlighted on the grid.
	referencedThings: [],

	highlightHoveredThing: function(thing) {

		this.hoveredThing = thing;
		this.drawGrid();

		var index;
		var analyzed = false;
		for(index = 0; index < this.world.gidget.runtime.analyzed.length; index++)
			if(this.world.gidget.runtime.analyzed[index] === thing)
				analyzed = true;

		if(analyzed)
			this.visualizeDecision(GIDGET.text.memory_analyzed(thing.name, thing.actions, thing.tags), true);
		else
			this.visualizeDecision(GIDGET.text.memory_unanalyzed(thing.name), true);

	},

	unhighlightHoveredThing: function() {

		this.hoveredThing = undefined;
		this.drawGrid();

		this.visualizeDecision(GIDGET.text.memory_unfocus(), true);

	},

	highlightToken: function(token, isGoal) {

		// Remove any current highlighting.
		$('.sourceLine').removeClass('runtimeReference');
		$('.sourceToken').removeClass('runtimeReference');

		// If there is a token, highlight it.
		if(isDef(token)) {

			// If the token is a goal, highlight the appropriate goal
			if(isGoal === true) {
				$('#goal' + this.goalNumberBeingExecuted + ' #sourceToken' + token.index).addClass('runtimeReference');
				$('#goal' + this.goalNumberBeingExecuted + ' #sourceLine' + token.line).addClass('runtimeReference');
			}
			// Otherwise, highlight the main code
			else {

/* 				console.log("Highlighting token # " + token.index); */

				$('#code #sourceToken' + token.index).addClass('runtimeReference');
				$('#code #sourceLine' + token.line).addClass('runtimeReference');
			}

		}

	},

	getCellSize: function() { return $('#grid').width() / this.world.grid[0].length; },

	// Consults the percentRemaining field, influencing where things are drawn on the grid.
	// Once the grid is drawn, it then invokes another timeout to draw again in another 25 ms.
	animate: function() {

		GIDGET.ui.percentRemaining -= 10;
		// If there's nothing left to animate, draw one last time.
		if(GIDGET.ui.percentRemaining < 0) {
			GIDGET.ui.world.resetThingDeltas();
			GIDGET.ui.percentRemaining = 0;
			GIDGET.ui.drawGrid();
		}
		// Otherwise, draw and then invoke another animate in 25 ms.
		else if(GIDGET.ui.percentRemaining > 0) {
			GIDGET.ui.drawGrid();
			setTimeout(GIDGET.ui.animate, 25);
		}

	},

	drawGrid: function() {

		var grid = this.world.grid;

		var canvas = document.getElementById('grid');
		var ctx = canvas.getContext('2d');
		var row, col;

		ctx.fillStyle = this.world.grndColor;
		ctx.fillRect(0, 0, canvas.width, canvas.height);
		ctx.lineWidth = 1;

		var cellSize = this.getCellSize();
		var things, thing;
		var i, x, y;

		function drawLevel(level) {

			var count = 0;
			for(row = 0; row < grid.length; row++) {
				for(col = 0; col < grid[row].length; col++) {

					things = grid[row][col];

					// Go through all of the things at this level and draw them.
					for(i = 0; i < things.length; i++) {

						thing = things[i];

						if(thing.level === level) {

							count++;
							thing.draw(ctx, cellSize);

						}

					}

				}

			}

			return count;

		}

		var currentLevel = 0;
		while(drawLevel(currentLevel) > 0)
			currentLevel++;

		// Now draw the text
		for(row = 0; row < grid.length; row++) {
			for(col = 0; col < grid[row].length; col++) {

				things = grid[row][col];

				// Start at 4 pixels above the cell
				var textYOffset = 4;

				// Go through all of the things at this level and draw them.
				for(i = 0; i < things.length; i++) {

					var thing = things[i];

					// Only draw labels for the things labeled.
					if(thing.labeled === true) {

						ctx.font = "10pt Arial";

						var animateRowOffset = 0, animateColumnOffset = 0;
						if(GIDGET.ui.percentRemaining > 0) {

							if(thing.rowDelta !== 0) animateRowOffset = -(GIDGET.ui.percentRemaining / 100.0) * cellSize * thing.rowDelta;
							if(thing.columnDelta !== 0) animateColumnOffset = -(GIDGET.ui.percentRemaining / 100.0) * cellSize * thing.columnDelta;

						}

						var textWidth = ctx.measureText(thing.name).width;
						var textXOffset =  -(textWidth - cellSize) / 2;

						ctx.fillStyle = "black";
						ctx.fillText(thing.name, thing.column * cellSize - 1 + textXOffset + animateColumnOffset, thing.row * cellSize - textYOffset - 1 + animateRowOffset);
						ctx.fillStyle = "white";
						ctx.fillText(thing.name, thing.column * cellSize + textXOffset + animateColumnOffset, thing.row * cellSize - textYOffset + animateRowOffset);

						textYOffset += 12;

					}

				}

			}

		}

		// Draw highlights around the referenced things
		if(isDef(this.referencedThings)) {
			for(var i = 0; i < this.referencedThings.length; i++) {
				ctx.strokeStyle = "rgb(255,0,0)";
				ctx.lineWidth = "4";
				ctx.strokeRect(this.referencedThings[i].column * cellSize, this.referencedThings[i].row * cellSize, cellSize, cellSize);
			}
		}

		// Draw a highlight around the highlighted thing, if there is one.
		if(isDef(this.hoveredThing)) {

			ctx.strokeStyle = "rgb(0,255,0)";
			ctx.lineWidth = "4";
			ctx.strokeRect(this.hoveredThing.column * cellSize, this.hoveredThing.row * cellSize, cellSize, cellSize);

		}

		// If Gidget is scanning something, make an animated green glow!
		if(isDef(this.animatingScanned)) {

			ctx.fillStyle = "rgb(0,255,0)";
			ctx.strokeStyle = "rgb(0,128,0)";
			ctx.lineWidth = "2";
			ctx.beginPath();
			ctx.moveTo(this.animatingScanned.column * cellSize + cellSize / 2, this.animatingScanned.row * cellSize + cellSize / 2);
			ctx.arc(
				this.animatingScanned.column * cellSize + cellSize / 2, // x
				this.animatingScanned.row * cellSize + cellSize / 2, 	// y
				cellSize / 2, 												// radius
				0.0, 													// start angle
				2 * Math.PI * (this.percentRemaining / 100.0), 			// end angle
				false);													// anticlockwise mode
			ctx.fill();
			ctx.stroke();
			ctx.closePath();

			if(this.percentRemaining > 10) {
				ctx.beginPath();
				ctx.lineWidth = "2";
				ctx.moveTo(this.world.gidget.column * cellSize + cellSize / 2, this.world.gidget.row * cellSize + cellSize / 2);
				ctx.lineTo(this.animatingScanned.column * cellSize + cellSize / 2, this.animatingScanned.row * cellSize + cellSize / 2);
				ctx.stroke();
				ctx.closePath();
			}

		}

		// If gidget is focused, overlay a transparent cloud over everything to represent his singular focus.
		if(isDef(this.world) && isDef(this.world.gidget) && this.world.gidget.runtime.focused.length > 0) {

			// This line commented out animates the fade, but there's no memory of whether the fade is new.
/* 			ctx.fillStyle = "rgba(0,0,0," + (0.8 * ((100.0 - this.percentRemaining) / 100.0)) + ")"; */
			ctx.fillStyle = "rgba(0,0,0,0.8)";

			var focus = this.world.gidget.runtime.focused[0];
			var focusRow = focus.row;
			var focusColumn = focus.column;
			var padding = 3;
			var size = cellSize + padding * 2;
			var focusLeft = focusColumn * cellSize - padding;
			var focusTop = focusRow * cellSize - padding;
			var focusRight = focusLeft + size - 1;
			var focusBottom = focusTop + size - 1;

			// Top three cells
			ctx.fillRect(0,0,focusLeft, focusTop);
			ctx.fillRect(focusLeft, 0, size, focusTop);
			ctx.fillRect(focusRight,0,canvas.width - focusRight + 1, focusTop);

			// Left and right middle cells
			ctx.fillRect(0,focusTop,focusLeft, size);
			ctx.fillRect(focusRight,focusTop,canvas.width - focusRight, size);

			// Bottom three cells
			ctx.fillRect(0,focusBottom,focusLeft, canvas.height - focusBottom);
			ctx.fillRect(focusLeft, focusBottom, size, canvas.height - focusBottom);
			ctx.fillRect(focusRight,focusBottom,canvas.width - focusRight + 1, canvas.height - focusBottom);

		}

	}

};
