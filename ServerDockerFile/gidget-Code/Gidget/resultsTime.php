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

	function getTimeX(row, kind) {

		var myString = "";
		for(var level in row.levelMetadata) {
			if(row.levelMetadata.hasOwnProperty(level)) {
				var data = row.levelMetadata[level];
				
				for(var i = 0; i < data.stepLog.length; i++) {
					if(data.stepLog[i].kind === kind)
						myString += "," + kind;
				}
			}
		}
		return myString;
	}

	function getTime(row) {
	
		var myString = "";
		var calc;
		var first = true;
		var count = 0;
		
		for(var level in row.levelMetadata) {
			if(row.levelMetadata.hasOwnProperty(level)) {
				var data = row.levelMetadata[level];
				if (first)
					{myString += "=" + data.endTime + "-"+ data.startTime + ""; first = false;}
				else
					{myString += ",=" + data.endTime + "-"+ data.startTime + "";}
				count++;
			}
		}
		
		for (var i = 18-count; i > 0; i--)
		{
			myString += ",";
		}
		
		return myString;
		//return calc;
	}

	
	$(document).ready(function() {
	
		$('#results').append("mturkcode,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,final\n"); 		
		
		var id, row;
		for(id in results) {
			if(results.hasOwnProperty(id)) {

				var row = results[id];			
				add(row.code);
				add(getTime(row));
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