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

// Get a book by Id - show bookName 
app.controller('AddBookCtrlr', function ($scope, $http) {
    $scope.list = [];

    $scope.addListItems = function (lstSach) {
        if (lstSach != null) {
            for (i = 0; i < lstSach.length; i++) {
                $scope.idBook = lstSach[i];
                $scope.addItem();
            }
        }
    }

    $scope.addItem = function () {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "http://localhost:64002/PhieuMuon/_GetBookItemById",
            params: { idBook: $scope.idBook }
        }).then(function (response) {
            if (response.data) {
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

// Get a book by Id - TraSachCtrlr  
app.controller('TraSachCtrlr', function ($scope, $http) {
    $scope.list = [];
    $scope.addItem = function () {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "http://localhost:64002/PhieuTra/GetThongTinPhieuTra",
            params: {
                idBook: $scope.idBook,
                soLuong: $scope.soLuong,
                idTrangThai: $scope.idTrangThai,
                idPM : $("idPM").val()
            }
        }).then(function (response) {
            if (response.data) {
                $scope.list.push(response.data);
                $scope.GetAllData();
            }
            else {
                alert("Dữ liệu không phù hợp");
            }
        }, function () {
            alert("Error Occur");
        })
    }
	
    //$scope.removeItem = function (x) {
    //    $scope.errortext = "";
    //    $scope.list.splice(x, 1);
    //};

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "http://localhost:64002/PhieuMuon/GetChiTietPhieuJSon",
            params: {
                idPM: $('#idPM').val(),
                soLuong: 0
            }
        }).then(function (response) {
            $scope.listGet = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

app.controller('SachMuonCtrlr', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "http://localhost:64002/PhieuMuon/GetChiTietPhieuJSon",
            params: {
                idPM: $('#idPM').val(),
                soLuong : 0
            }
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});
