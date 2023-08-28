var SCENES = {

	Sprite: function(url, width, height) {
	
		this.image = GIDGET.ui.media.getImage(url);
		this.width = width;
		this.height = height;
		
	},

	Scene: function(length) {
	
		this.length = length;
		
		this.sprites = [];
		this.sounds = [];
		
		this.addSprite = function(sprite, time, coord) {
			
			// time == (begin, end); coord = (x-pos, y-pos)
			this.sprites.push({ sprite: sprite, time: time, coord: coord });		
			
		};
		
		this.addSound = function(url, begin) {
		
			this.sounds.push({ url: url, begin: begin});
		
		},
		
		this.draw = function(canvas, time) {
		
			var ctx = canvas.getContext('2d');
			var i;
			var sprite, begin, end;
	
			ctx.fillStyle = 'rgb(0,0,0)';
			ctx.clearRect(0, 0, canvas.width, canvas.height);

			for(i = 0; i < this.sounds.length; i++) {
			
				if(this.sounds[i].begin <= time) {
					
					GIDGET.ui.media.playSound(this.sounds[i].url);
					this.sounds.splice(i, 1);
					i--;
				
				}
			
			}			
			
			for(i = 0; i < this.sprites.length; i++) {
			
				sprite = this.sprites[i].sprite;
				begin = this.sprites[i].time[0];
				end = this.sprites[i].time[1];
				xpos = this.sprites[i].coord[0];
				ypos = this.sprites[i].coord[1];
								
				// If this is visible, draw the sprite.
				if(begin < time && time < end) {
	
					if(sprite.width > 0 && sprite.height > 0 && isDef(sprite.image))
						ctx.drawImage(sprite.image, xpos, ypos, sprite.width, sprite.height);	
				
				}
			
			}
		
		};
	
	},

	Movie: function() {
	
		this.scenes = [];
		
		this.sceneStartTime = 0;
		this.sceneTimeElapsed = undefined;
		
		this.intervalID = undefined;
		
		this.currentScene = undefined;
		
		this.addScene = function(scene) {
		
			this.scenes.push(scene);
		
		};
		
		this.step = function(canvas, whenDone) {

			// What scene are we on?
			var scene = this.scenes[this.currentScene];

			// If this scene is over, move to the next scene
			if(this.currentScene === 0 && this.sceneTimeElapsed === undefined) {
			
				this.sceneStartTime = (new Date()).getTime();
				this.sceneTimeElapsed = 0;
			
			}
			
			if(this.sceneTimeElapsed > scene.length) {
			
				this.sceneStartTime = (new Date()).getTime();
				this.sceneTimeElapsed = 0;
				this.currentScene++;
			
			}			

			// Update times
			this.sceneTimeElapsed = (new Date()).getTime() - this.sceneStartTime;
	
			
			// Draw the current scene.
			scene.draw(canvas, this.sceneTimeElapsed);			
		
			// If this movie is over, end the animation
			if(this.currentScene >= this.scenes.length) {
			
				clearInterval(this.intervalID);
				whenDone.call();
			
			}
		
		};
		
		this.play = function(canvas, whenDone) {
		
			this.sceneTimeElapsed = undefined;
			this.currentScene = 0;
			var thisMovie = this;
			this.intervalID = setInterval(function() { thisMovie.step(canvas, whenDone); }, 40);
		
		};
	
	}

}