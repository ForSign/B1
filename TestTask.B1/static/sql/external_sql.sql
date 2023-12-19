SELECT *
FROM ( SELECT SUM(decimal_value) as "SUM"
	   FROM `B1`.`task_1`) AS A
JOIN ( SELECT AVG(dd.double_value) as "AVG"
	   FROM ( SELECT task_1.double_value, @rownum:=@rownum+1 as `row_number`, @total_rows:=@rownum
              FROM task_1, (SELECT @rownum:=0) r
			  WHERE task_1.double_value is NOT NULL
			  ORDER BY task_1.double_value) as dd
       WHERE dd.row_number IN ( FLOOR((@total_rows+1)/2), FLOOR((@total_rows+2)/2))) AS B;