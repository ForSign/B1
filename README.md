
# B1

## Test task

### 1.0
Switching between task options available using menubar:  
![alt text](https://github.com/ForSign/B1/raw/master/img/1.png)

### 1.1
When generating files user prompt for input of file and line count aswell as folder where to save generated files  
![alt text](https://github.com/ForSign/B1/raw/master/img/2.png)

If there are other files in folder user would have another prompt  
![alt text](https://github.com/ForSign/B1/raw/master/img/3.png)

At center of application is Log screen displaying information and progress for certain tasks  
![alt text](https://github.com/ForSign/B1/raw/master/img/4.png)

### 1.2
When merging files user is able to put filter of which lines exclude.  
WARNING This purges original files (supposedly for as task 1.2 states)  
![alt text](https://github.com/ForSign/B1/raw/master/img/5.png)

Filter is dynamically expandable  
![alt text](https://github.com/ForSign/B1/raw/master/img/6.png)

After pressing ***Purge and Merge*** select files you wish to perform purge and future merge on  
![alt text](https://github.com/ForSign/B1/raw/master/img/7.png)

Then select save filename  
![alt text](https://github.com/ForSign/B1/raw/master/img/8.png)

Purge progress:  
![alt text](https://github.com/ForSign/B1/raw/master/img/9.png)

Purge finish and merge start progress:  
![alt text](https://github.com/ForSign/B1/raw/master/img/10.png)

### 1.3
User prompted for Purge and Merge before inserting to sql  
![alt text](https://github.com/ForSign/B1/raw/master/img/11.png)

Select files to insert rows from (as task states from files)  
It will then merge it into temporary file from which import will be performed  
WARNING All rows that cannot be parsed will be ignored  
![alt text](https://github.com/ForSign/B1/raw/master/img/12.png)

Insertion progress  
![alt text](https://github.com/ForSign/B1/raw/master/img/13.png)

Smaller example of how progress goes  
![alt text](https://github.com/ForSign/B1/raw/master/img/14.png)

### 1.4
Counts Median of Double values and Sum of Decimal rows that are already inserted in db  
![alt text](https://github.com/ForSign/B1/raw/master/img/15.png)

## 2.0
Task 2 options:  
![alt text](https://github.com/ForSign/B1/raw/master/img/16.png)

### 2.1
Select file to parse into sql querries and insert them into db  
![alt text](https://github.com/ForSign/B1/raw/master/img/17.png)

File uploaded popup  
![alt text](https://github.com/ForSign/B1/raw/master/img/18.png)

### 2.2 - 2.3
Show uploaded files in below maneur:  
BankName - - - - - Date  
![alt text](https://github.com/ForSign/B1/raw/master/img/19.png)

Selecting file will display its content  
![alt text](https://github.com/ForSign/B1/raw/master/img/20.png)

# DataBase diagramm
![alt text](https://github.com/ForSign/B1/raw/master/img/db.png)