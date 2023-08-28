GIDGET.ui.media = {

	disableSounds: false,

	mediaToLoad: 0,
	mediaRemainingToLoad: 0,

	// The hash table of image objects
	images: [],
	sounds: [],

	// Retrieve an image object by url
	getImage: function(url) {
	
		if(this.images.hasOwnProperty(url))
			return this.images[url];
		else
			return undefined;
	
	},
	
	playSound: function(command) {

		// Play sounds as long as the learner didn't press the (runTo)"End" button.
		if(!this.disableSounds) {

			var sound = this.sounds[command];
			if(isDef(sound))
				sound.play();
			else
				console.error("There was no sound named " + command);
			
		}
	
	},
	
	loadSound: function(name) {

		// Make a sound	
		var sound = document.createElement('audio');

		// Listen to success and failure		
		var handleLoad = function() { 
			GIDGET.ui.media.mediaRemainingToLoad--;
			GIDGET.ui.media.sounds[name] = this;
			GIDGET.ui.media.notifyOfProgress();
		}
		
		$(sound).bind("loadeddata", handleLoad);
		
		$(sound).bind("error", function() {
			GIDGET.ui.media.mediaRemainingToLoad--;
			GIDGET.ui.media.notifyOfProgress();
		});

		// Indicate which file
		sound.setAttribute('src', 'media/sfx/' + name + '.wav');

		// Add the sound to the body, so that onload events get fired.
		$('body').append(sound);

		// Add it to the total
		this.mediaToLoad++;
		this.mediaRemainingToLoad++;
		
		// Load the sound
		sound.load();	
	
	},

	loadImage: function(url) {
	
		var image = new Image();

		// Increment how many images are remaining to load
		this.mediaToLoad++;
		this.mediaRemainingToLoad++;

		// Decrement on success or failure
		image.onload = function () {
			GIDGET.ui.media.mediaRemainingToLoad--;
			// Store the image in the images table
			GIDGET.ui.media.images[url] = image;
			// Notify callback of progress
			GIDGET.ui.media.notifyOfProgress();
		};
		image.onerror = function () {
			GIDGET.ui.media.mediaRemainingToLoad--;
			// Notify callback of progress
			GIDGET.ui.media.notifyOfProgress();
		};
		
		// Begin loading
		image.src = url;
	
	},
	
	progressCallback: undefined,
	
	notifyOfProgress: function() {
	
		if(isDef(this.progressCallback))
			this.progressCallback.call(this, this.mediaRemainingToLoad, this.mediaToLoad);

	},
	
	loadMedia: function(onProgress) {
	
		this.progressCallback = onProgress;
		
		/*
		this.loadImage("media/scene/bg01.png");
		this.loadImage("media/scene/bg02.png");
		this.loadImage("media/scene/bg03.png");
		this.loadImage("media/scene/sky.png");
		this.loadImage("media/scene/ground.png");
		
		this.loadImage("media/scene/boom.png");
		
		this.loadImage("media/scene/gidget.sad.png");
		this.loadImage("media/scene/gidget.confident.png");
		this.loadImage("media/scene/gidget.control.png");
		this.loadImage("media/scene/gidget.eager.png");				
		this.loadImage("media/scene/gidget.surprised.png");
		this.loadImage("media/scene/gidget.smashExp.png");
		this.loadImage("media/scene/rock.png");
		
		this.loadImage("media/scene/heli-westbound.png");
		this.loadImage("media/scene/heli-westboundExp.png");
		this.loadImage("media/scene/heli-westboundControl.png");				
		this.loadImage("media/scene/heli-eastbound.png");

		this.loadImage("media/scene/goop.default.png");
		this.loadImage("media/scene/kitten.default.png");
		this.loadImage("media/scene/cat.default.png");
		this.loadImage("media/scene/puppy.default.png");
		this.loadImage("media/scene/dog.default.png");
		this.loadImage("media/scene/piglet.default.png");
		this.loadImage("media/scene/bird.default.png");

		*/



	
		// Action Sounds
		this.loadSound("analyze");
		this.loadSound("drop");
		this.loadSound("grab");
		this.loadSound("goto");
		this.loadSound("scan");
		this.loadSound("focusIn");
		
		// Goal Evaluation Sounds
		this.loadSound("goal_checkFailure");
		this.loadSound("goal_finalSuccess");
		this.loadSound("goal_checkSuccess");
		this.loadSound("goal_finalFailure");
		
		// Event Sounds
		this.loadSound("energyUp");
		this.loadSound("energyDown");
		this.loadSound("error");
		this.loadSound("parserErrorExp");
		this.loadSound("parserErrorCtrl");
		this.loadSound("errorExp");
	
	}

};