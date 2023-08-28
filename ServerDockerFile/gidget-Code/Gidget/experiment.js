GIDGET.experiment = {

	condition: "male",	// control, male, female
	
	adapt: false,
	
	verbose: false,
	
	bonusPerLevel: 0.10,
	
	isControl: function() {
	
		if (this.condition === "control")
			return true;
		
		return false;
	
	},
	
	isMale: function() {	
	
		if (this.condition === "male")
			return true;
		
		return false;
	
	},
	
	isFemale: function() {	
	
		if (this.condition === "female")
			return true;
		
		return false;
	
	},
	
	isExperimental: function() {
		
		if ((this.condition === "male") || (this.condition === "female"))
			return true;
		
		return false;
	
	},
	
	loadExpCondition: function() {
		
		// Set the current experimental state from localStorage
		this.condition = localStorage.getItem('expCondition').replace(/['"]/g,'');
		this.adapt = localStorage.getItem('adaptCondition') === "true";
		
	},
	
	saveExpCondition: function() {
		
		// Save the current experimental state to localStorage
		localStorage.setItem('expCondition', this.condition);
		localStorage.setItem('adaptCondition', this.adapt);	

	},

};