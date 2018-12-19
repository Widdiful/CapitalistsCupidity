<?php
	$con = mysqli_connect('localhost', 'root', 'root', 'mobiledev');

	// check for successful connection
	if (mysqli_connect_errno()){
		echo "1: Connection failed";
		exit();
	}

	$playerID	= $_POST["playerID"];
	$companyName= $_POST["companyName"];
    $score      = $_POST["score"];
    $time       = $_POST["time"];
    $tableName  = $_POST["tableName"];

	$sql = "SELECT id FROM " . $tableName . " WHERE player_id = '" . $playerID . "'";
    $result = mysqli_query($con, $sql);
    
    if (mysqli_num_rows($result) > 0){ // Score already exists
        while($row = mysqli_fetch_assoc($result)){
            if ($row["score"] < $score){
                $sql = "UPDATE " . $tableName . " SET score = '" . $score . "' WHERE player_id = '" . $playerID . "'";
                mysqli_query($con, $sql);
                $sql = "UPDATE " . $tableName . " SET company_name = '" . $companyName . "' WHERE player_id = '" . $playerID . "'";
                mysqli_query($con, $sql);
            }
        }
        echo("1");
    }
    else{ // Score doesn't exist
        $sql = "INSERT INTO highscoresfree (id, player_id, company_name, score) VALUES (NULL, '" . (int) $playerID . "', '" . $companyName . "', '" . (int) $score . "')";
        mysqli_query($con, $sql) or die("2: Failed to create user");
        echo("1");
    }
    echo("0");
?>