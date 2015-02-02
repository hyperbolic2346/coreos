<?php
//define('SOCKET_COMMUNICATION', true);

//	if ($wilson && $brigman) {
		$url = "http://10.0.1.25/gate";
/*	} else if ($wilson) {
		$url = "http://10.0.1.25/gate/0";
	} else if ($brigman) {
		$url = "http://10.0.1.25/gate/1";
	} else {
		exit();
	}*/

	$curl = curl_init();

	curl_setopt($curl, CURLOPT_URL, $url);
	curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);

	$buf = curl_exec($curl);

echo $buf;
//return;

echo "\r\n";
// parse the response
$gates = json_decode($buf, true);
echo "gates 0 is ".$gates[0]['state'];
echo "\r\n";
echo json_encode($gates);

?>
