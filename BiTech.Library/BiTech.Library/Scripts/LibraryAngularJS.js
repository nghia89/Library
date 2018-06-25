// Define the `LibraryApp` module
var app = angular.module('LibraryApp', []);

// Define the `BookGenresCtrlr` controller on the `LibraryApp` module
app.controller('BookGenresCtrlr', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "http://localhost:64002/TheLoaiSach/Get_AllTheLoaiSach"
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
            url: "http://localhost:64002/NhaXuatBan/Get_AllNhaXuatBan"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

// Add IdBook to list
//app.controller('AddBookCtrlr', function ($scope, $http) {
//    $scope.list = ["A", "B", "C"];
//    $scope.addItem = function () {
//        $scope.errortext = "";
//        if (!$scope.addMe) { return; }
//        if ($scope.list.indexOf($scope.addMe) == -1) {
//            $scope.list.push($scope.addMe);
//        } else {
//            $scope.errortext = "The item is already in your shopping list.";
//        }
//    }
//    $scope.removeItem = function (x) {
//        $scope.errortext = "";
//        $scope.list.splice(x, 1);
//    }
//});

app.controller('AddBookCtrlr', function ($scope, $http) {
    $scope.list = [];
    
    $scope.addItem = function () {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "http://localhost:64002/PhieuMuon/_GetBookItemById",
            params: { idBook: $scope.addMe }
        }).then(function (response) {
            $scope.list.push(response.data);
        }, function () {
            alert("Error Occur");
        })
    }  
    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };
});