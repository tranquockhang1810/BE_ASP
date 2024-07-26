CREATE database TaskManagerDB;
GO

use TaskManagerDB;
GO

-- Create user table
-- create table Users
-- (
--   id int primary key identity(1,1),
--   name varchar(50) not null,
--   phone varchar(50),
--   email varchar(50) not null,
--   password varchar(50) not null
-- );
-- GO

-- create tasks table
-- create table Tasks
-- (
--   id int primary key identity(1,1),
--   name varchar(50) not null,
--   description varchar(50),
--   createdDate datetime default getdate(),
--   dueDate datetime,
--   priority int default 0, 
--   -- 0: low, 1: medium, 2: high
--   status int default 0,
--   -- 0: open, 1: closed
--   userId int not null,
--   foreign key (userId) references Users(id)
-- );
-- GO

-- insert into Users
--   (name, phone, email, password) values 
--   (N'admin', '0987654321', 'admin@gmai.com', 'admin'),
--   (N'khangtran', '0123456789', 'khangtran@gmail.com', 'tranquockhang1');
-- GO

insert into Tasks
  (name, description, dueDate, priority, userId) values
  ('Task 1', 'Task 1 description', '2022-01-01 00:00:00', 0, 1),
  ('Task 2', 'Task 2 description', '2022-01-01 00:00:00', 1, 1)
GO

SELECT * FROM Tasks