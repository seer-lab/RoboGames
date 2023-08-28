<html>

<head>
	<title>Gidget Study Results</title>

	<script type='text/javascript' src='lib/jquery.js'></script>
	<script type='text/javascript' src='runtime.js'></script>
	<script type='text/javascript' src='experiment.js'></script>
	
	<script type='text/javascript'>
	
	var results = {
	
	<?php 

		// Report all errors
		error_reporting(-1);
	
		function endsWith($haystack,$needle,$case=true) {
		    if($case){return (strcmp(substr($haystack, strlen($haystack) - strlen($needle)),$needle)===0);}
		    return (strcasecmp(substr($haystack, strlen($haystack) - strlen($needle)),$needle)===0);
		}
		
		$myDirectory = opendir("results/");
		
		// get each entry
		$indexCount = 0;
		while($entryName = readdir($myDirectory)) {
			$dirArray[] = $entryName;
			$indexCount++;
		}
		
		// close directory
		closedir($myDirectory);	
	
		$entries = array();
	
		for($index=0; $index < $indexCount; $index++) {
	        if(endsWith("$dirArray[$index]", ".json")) {
				$data = "";
				$filename = $dirArray[$index];
				$id = substr($filename, 0, strlen($filename) - 5);
				$file_handle = fopen("results/$filename", "r");
				if($file_handle === FALSE)
					exit("Couldn't open $filename");
				while (!feof($file_handle)) {
				   $line = fgets($file_handle);
				   $data = $data . $line;
				}
				fclose($file_handle);
				
				$entries[$id] = $data;
				
			}
		}
		
		
		foreach ( $entries as $id => $data )
			echo "'$id' : $data,\n";
	
	?>
	
	}
	
	function add(text) {
		$('#results').append(text + ",");
	}
	function line() {
		$('#results').append("\n");
	}
		
	function levelsCompleted(row) {
	
		var count = 0;
		for(var level in row.levelMetadata) {
			if(row.levelMetadata.hasOwnProperty(level)) {
				var data = row.levelMetadata[level];
				if(data.passed === true)
					count++;			
			}
		}
		return count;
	
	}
	
	function countSteps(row, kind) {

		var count = 0;
		for(var level in row.levelMetadata) {
			if(row.levelMetadata.hasOwnProperty(level)) {
				var data = row.levelMetadata[level];
				
				for(var i = 0; i < data.stepLog.length; i++) {
					if(data.stepLog[i].kind === kind)
						count++;
				}
			}
		}
		return count;
	}

	
	$(document).ready(function() {
	
		$('#results').append("id,condition,currentLevel,mturkcode,bonus,levelsCompleted,steps,lineSteps,plays,ends,gender,age,education,exp1,exp2,exp3,exp4,exp5,ex6,enjoyment,help\n"); 		
		
/*
			condition: GIDGET.experiment.condition,
			currentLevel: localStorage.getItem('currentLevel'),
			levelsPassed: this.getNumberOfLevelsPassed(),
			code: password,
			levelMetadata: getLocalStorageObject('levelMetadata'),
				
				gender: this.radioEmpty("gender"),
				age: this.removeSpecialCharacters($('input[name=age]').val()),
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
*/

		
		var id, row;
		for(id in results) {
			if(results.hasOwnProperty(id)) {

				var row = results[id];				

				add(id);
				add(row.condition);
				add(row.currentLevel);
				add(row.code);
				add((levelsCompleted(row) * GIDGET.experiment.bonusPerLevel).toFixed(2));
				add(levelsCompleted(row));
				add(countSteps(row, "step"));
				add(countSteps(row, "line"));
				add(countSteps(row, "play"));
				add(countSteps(row, "end"));
				add(row.survey.gender);
				add(row.survey.age);
				add(row.survey.education);
				add(row.survey.experience1);
				add(row.survey.experience2);
				add(row.survey.experience3);
				add(row.survey.experience4);
				add(row.survey.experience5);
				add(row.survey.experience6);
				add(row.survey.enjoyment);
				add(row.survey.recommend);
				add(row.survey.dialogue);
				add(row.survey.avatar);
				add(row.survey.experience);
				add(row.survey.whyQuit);
				add(row.survey.whyMore);
				line();
			
			}
		}
	
	});
	

	</script>

</head>
<body>

	<pre id='results'></pre>

</body>

</html>