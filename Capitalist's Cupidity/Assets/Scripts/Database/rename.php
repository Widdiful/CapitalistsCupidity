<?php
	$con = mysqli_connect('localhost', 'root', 'root', 'mobiledev');

	// check for successful connection
	if (mysqli_connect_errno()){
		echo "1: Connection failed";
		exit();
	}

	$playerID	= $_POST["playerID"];
	$newName	= $_POST["newName"];

	// add new user
	$query = "UPDATE players SET name = '" . $newName . "' WHERE id = '" . $playerID . "'";
	mysqli_query($con, $query) or die("2: Failed to rename user");

	echo "0";
?>