// Define the `LibraryApp` module
var app = angular.module('LibraryApp', []);

// Define the `BookGenresCtrlr` controller on the `LibraryApp` module
app.controller('BookGenresCtrlr', function ($scope, $http) {
    $scope.students = [];
    $scope.statistics = [];

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
    $scope.students = [];
    $scope.statistics = [];

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

// nhập sách
app.controller('BookCtrlr', function ($scope, $http) {

        $scope.list = [];
        $scope.addItem = function () {
            $scope.errortext = "";
            $http({
                method: "get",
                url: "http://localhost:64002/PhieuNhapSach/_GetBookItemById",
                params: {
                    idBook: $scope.idBook,
                    soLuong: $scope.soLuong,
                    idTrangThai: $scope.idTrangThai
                }
            }).then(function (response) {
                if (response.data != null) {
                    $scope.list.push(response.data);
                }
                else {
                    alert("Mã sách không phù hợp");
                }
            }, function () {
                alert("Error Occur");
            })
        }
        $scope.removeItem = function (x) {
            $scope.errortext = "";
            $scope.list.splice(x, 1);
        };
     
});
//xuất sách
app.controller('ExportBookCtrlr', function ($scope, $http) {

    $scope.list = [];
    $scope.addItema = function () {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "http://localhost:64002/PhieuXuatSach/_GetBookItemById",
            params: {
                idBook: $scope.idBook,
                soLuong: $scope.soLuong,
                idTrangThai: $scope.idTrangThai,
                idLyDo:$scope.idLyDo
            }
        }).then(function (response) {
            if (response.data != null) {
                $scope.list.push(response.data);
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function () {
            alert("Error Occur");
        })
    }
    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };

});
