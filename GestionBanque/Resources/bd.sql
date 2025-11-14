DROP TABLE IF EXISTS compte;
DROP TABLE IF EXISTS client;

CREATE TABLE client
(
	id			INTEGER PRIMARY KEY,
	nom			TEXT	NOT NULL,
	prenom		TEXT	NOT NULL,
	courriel	TEXT	NOT NULL UNIQUE
);

CREATE TABLE compte
(
	id			INTEGER PRIMARY KEY,
	no_compte   TEXT	NOT NULL UNIQUE,
	balance		REAL	NOT NULL,
	client_id	INTEGER NOT NULL,
	FOREIGN KEY (client_id) REFERENCES client (id) ON DELETE CASCADE ON UPDATE CASCADE
);


INSERT INTO client VALUES (NULL, 'Amar', 'Quentin', 'quentin@gmail.com');
INSERT INTO client VALUES (NULL, 'Agère', 'Tex', 'tex@gmail.com');
INSERT INTO client VALUES (NULL, 'Vigote', 'Sarah', 'sarah@gmail.com');

INSERT INTO compte VALUES (NULL, '9864', 831.76, 1);
INSERT INTO compte VALUES (NULL, '2370', 493.04, 1);
INSERT INTO compte VALUES (NULL, '7640', 634.73, 2);
INSERT INTO compte VALUES (NULL, '7698', 906.72, 3);


