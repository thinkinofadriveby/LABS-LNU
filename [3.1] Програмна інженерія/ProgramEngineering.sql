CREATE TABLE ticket(
    id serial NOT NULL PRIMARY KEY,
    placeNumber int NOT NULL,
    typeOfPlaceId serial NOT NULL,
    receivingTime datetime NOT NULL,
    trainId serial NOT NULL,
    userId serial NOT NULL,
    price float NOT NULL,
    FOREIGN KEY (typeOfPlaceId) REFERENCES typeOfPlace(id) ON DELETE CASCADE,
    FOREIGN KEY (trainId) REFERENCES train(id) ON DELETE CASCADE,
    FOREIGN KEY (userId) REFERENCES user(id) ON DELETE CASCADE
);

CREATE TABLE typeOfPlace(
    id serial NOT NULL PRIMARY KEY,
    placeClass char NOT NULL
);

CREATE TABLE train(
    id serial NOT NULL PRIMARY KEY,
    numberOfPlaces int NOT NULL,
    trainDescriptionId serial NOT NULL,
    information char NOT NULL,
    FOREIGN KEY (trainDescriptionId) REFERENCES trainDescription(id) ON DELETE CASCADE
);

CREATE TABLE user(
    id serial NOT NULL PRIMARY KEY,
    login char NOT NULL,
    password char NOT NULL,
    isAuthorized bool NOT NULL,
    email char NOT NULL,
    moneyOnBill float NOT NULL
);

CREATE TABLE placesInTrain(
    id serial NOT NULL PRIMARY KEY,
    trainId serial NOT NULL,
    firstClassTicketsLost int NOT NULL,
    secondClassTicketsLose int NOT NULL,
    thirdClassTicketsLost int NOT NULL,
    FOREIGN KEY (trainId) REFERENCES train(id) ON DELETE CASCADE
);

CREATE TABLE trainDescription(
    id serial NOT NULL PRIMARY KEY,
    Date datetime NOT NULL,
    startPoint char NOT NULL,
    startTime time NOT NULL,
    endPoint char NOT NULL,
    endTime time NOT NULL
);