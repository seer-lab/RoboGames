<?php 

	// Report all errors
	error_reporting(-1);

	// Get the JSON to save
	$data = $_POST["data"];
	$url = '199.212.33.3:8082/saveData';

	$options = array(
		'http' => array(
			'header' => "Content-Type: application/json\r\n",
			'method' => 'POST',
			'content' => http_build_query($data)
		)
	);

	$context = stream_context_create($options);
	$results = file_get_contents($url, false, $context);
	if($result === FALSE){/* ERROR HANDLE */}
	
	var_dump($result);

	echo "Saved.";

?>
