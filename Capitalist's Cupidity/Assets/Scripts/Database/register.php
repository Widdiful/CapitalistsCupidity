<?php
	$con = mysqli_connect('188.121.44.166', 'cupidity', 'Cupidity1!', 'cupidity');

	// check for successful connection
	if (mysqli_connect_errno()){
		echo "1: Connection failed";
		exit();
	}

	// add new user
	$query = "INSERT INTO players (name) VALUES ('New player')";
	mysqli_query($con, $query) or die("2: Failed to create user");
	$last_id = mysqli_insert_id($con);
	echo "ID" . $last_id;
?>