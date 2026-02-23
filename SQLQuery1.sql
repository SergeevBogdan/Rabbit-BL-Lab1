-- Подключаем твой файл как постоянную базу
CREATE DATABASE RabbitDB 
ON (NAME = RabbitDB_Data, 
    FILENAME = 'C:\Users\AceR\Desktop\Rabbit-Lab - 4\Business logic - rabbit\Database1.mdf')
LOG ON (NAME = RabbitDB_Log, 
    FILENAME = 'C:\Users\AceR\Desktop\Rabbit-Lab - 4\Business logic - rabbit\Database1_log.ldf');