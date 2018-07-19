﻿
var app = angular.module('LibraryApp', ['ngTagsInput']);

// Define the `BookGenresCtrlr` controller on the `LibraryApp` module
app.controller('BookGenresCtrlr', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/TheLoaiSach/Get_AllTheLoaiSach"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

// Define the `PublishersCtrlr` controller on the `LibraryApp` module
app.controller('PublishersCtrlr', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/NhaXuatBan/Get_AllNhaXuatBan"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

app.controller('KeSach', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/KeSach/GetAll"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

app.controller('NgonNgu', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/NgonNgu/GetAll"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

app.controller('TacGiaSelectorCtrlr', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/TacGia/GetAll"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

app.controller('MainCtrl', function ($scope, $http) {
    // An array of strings will be automatically converted into an 
    // array of objects at initialization
    $scope.superheroes = [
      'Batman',
      'Superman',
      'Flash',
      'Iron Man',
      'Hulk',
      'Wolverine',
      'Green Lantern',
      'Green Arrow',
      'Spiderman'
    ];

    $scope.log = [];

    $scope.loadSuperheroes = function (query) {
        // An arrays of strings here will also be converted into an
        // array of objects
        return $http.get('superheroes.json');
    };

    $scope.tagAdded = function (tag) {
        $scope.log.push('Added: ' + tag.text);
    };

    $scope.tagRemoved = function (tag) {
        $scope.log.push('Removed: ' + tag.text);
    };
});

app.controller('TacGiaFinder', function ($scope, $http) {

    $scope.listtg = [];

    $scope.loadData = function (query) {
        return $http({
            method: "get",
            url: "/TacGia/FindTacGia",
            params: { query: query }
        }).then(function (response) {
            return JSON.parse(response.data);
        }, function () {
            alert("Error Occur");
        })

    };

});
