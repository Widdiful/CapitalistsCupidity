<?php
	$con = mysqli_connect('localhost', 'root', 'root', 'mobiledev');

	// check for successful connection
	if (mysqli_connect_errno()){
		echo "Error 1: Connection failed";
		exit();
	}

	$table = $_POST["tableName"];
    
    $sql = "SELECT t2.name, t1.company_name, t1.score FROM ".$table." t1 JOIN players t2 ON t1.player_id = t2.id ORDER BY t1.score DESC";
    if ($table == "highscorestime"){
        $sql = str_replace("DESC", "ASC", $sql);
    }
    
    $result = mysqli_query($con, $sql);
    
    if (mysqli_num_rows($result) > 0){
        while($row = mysqli_fetch_assoc($result)){
            echo "player:".$row['name']."|company:".$row['company_name']."|.score:".$row['score'].";";
        }
    }
    else{
        echo "Error 2: No rows in table.";
    }
?>