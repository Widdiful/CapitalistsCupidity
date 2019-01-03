<?php
	$con = mysqli_connect('188.121.44.166', 'cupidity', 'Cupidity1!', 'cupidity');

	// check for successful connection
	if (mysqli_connect_errno()){
		echo "1: Connection failed";
		exit();
	}

	$playerID	= $_POST["playerID"];

	$sql = "SELECT name FROM players WHERE id = '" . $playerID . "'";
    $result = mysqli_query($con, $sql);
    
    if (mysqli_num_rows($result) > 0){
        while($row = mysqli_fetch_assoc($result)){
            echo $row['name'];
        }
    }
?>