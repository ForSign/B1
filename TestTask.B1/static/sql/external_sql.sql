SELECT * 
FROM

(
SELECT SUM(DecimalValue) as "SUM"
FROM Task_1
) t1

INNER JOIN

(
SELECT AVG(DoubleValue) AS "AVG"
FROM (SELECT DoubleValue
      FROM Task_1
	  ORDER BY DoubleValue                           -- Order to find values at nearest to center
      LIMIT 2 - (SELECT COUNT(*) FROM Task_1) % 2    -- If there is even rows compute average between them
      OFFSET (SELECT (COUNT(*) - 1) / 2
              FROM Task_1))
) t2;