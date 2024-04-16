--TASK 1 SQL QUERY STATEMENT

 CREATE TABLE StudentCourses (
  StudentId INT NOT NULL,
  CourseId varchar(20) NOT NULL,
  SemesterId VARCHAR(6) NOT NULL,
  [CreatedOn] datetimeoffset(7) NOT NULL,
  [LastModifiedOn] datetimeoffset(7) NOT NULL,
  PRIMARY KEY (StudentId, CourseId, SemesterId),
  FOREIGN KEY (StudentId) REFERENCES Students(Id),
  FOREIGN KEY (CourseId) REFERENCES Courses(Id),
  FOREIGN KEY (SemesterId) REFERENCES Semesters(Id)
);


INSERT INTO Courses (Id, Description, ProfessorId, SemesterId, CreatedOn, LastModifiedOn)
VALUES ('TLP-201-24','Telepathy, Advanced;24 ', 2,'2024-1', '2024-04-16 11:59:57.0761237 -03:00', '2024-04-16 11:59:57.0761237 -03:00');

INSERT INTO Courses (Id, Description, ProfessorId, SemesterId, CreatedOn, LastModifiedOn)
VALUES ('POT-101-24','Potions - 101; 24', 1,'2024-1', '2024-04-16 11:59:57.0761237 -03:00', '2024-04-16 11:59:57.0761237 -03:00');

  -- Student 1, Course 'TLP-201-23', Semester '2024-1'
INSERT INTO StudentCourses (StudentId, CourseId, SemesterId, CreatedOn, LastModifiedOn)
VALUES (1, 'TLP-201-24', '2024-1','2024-04-16 11:59:57.0761237 -03:00', '2024-04-16 11:59:57.0761237 -03:00');

-- Student 1, Course 'POT-101-23', Semester '2024-1'
INSERT INTO StudentCourses (StudentId, CourseId, SemesterId, CreatedOn, LastModifiedOn)
VALUES (1, 'POT-101-24', '2024-1','2024-04-16 11:59:57.0761237 -03:00', '2024-04-16 11:59:57.0761237 -03:00');

-- Student 2, Course 'TLP-201-23', Semester '2024-1'
INSERT INTO StudentCourses (StudentId, CourseId, SemesterId, CreatedOn, LastModifiedOn)
VALUES (2, 'TLP-201-24', '2024-1','2024-04-16 11:59:57.0761237 -03:00', '2024-04-16 11:59:57.0761237 -03:00');

-- Student 2, Course 'POT-101-23', Semester '2024-1'
INSERT INTO StudentCourses (StudentId, CourseId, SemesterId, CreatedOn, LastModifiedOn)
VALUES (2, 'POT-101-24', '2024-1','2024-04-16 11:59:57.0761237 -03:00', '2024-04-16 11:59:57.0761237 -03:00');

-- Student 3, Course 'TLP-201-23', Semester '2023-2'
INSERT INTO StudentCourses (StudentId, CourseId, SemesterId, CreatedOn, LastModifiedOn)
VALUES (3, 'TLP-201-23', '2023-2','2024-04-16 11:59:57.0761237 -03:00', '2024-04-16 11:59:57.0761237 -03:00');