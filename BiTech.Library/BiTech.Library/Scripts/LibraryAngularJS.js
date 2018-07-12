/// <reference path="angular.js" />
// Define the `LibraryApp` module

var app = angular.module('LibraryApp', []);

// Define the `BookGenresCtrlr` controller on the `LibraryApp` module
app.controller('BookGenresCtrlr', function ($scope, $http) {

    $scope.GetAllData = function (id) {
        $http({
            method: "get",
            url: "/TheLoaiSach/Get_AllTheLoaiSach"
        }).then(function (response) {
            $scope.list = response.data;
            for (i = 0; i < response.data.length ; i++) {
                if (response.data[i].Id == id) {
                    $scope.IdTLS = response.data[i];
                }
            }
        }, function () {
            alert("Error Occur");
        })
    };
});

// Define the `PublishersCtrlr` controller on the `LibraryApp` module
app.controller('PublishersCtrlr', function ($scope, $http) {

    $scope.GetAllData = function (id) {
        $http({
            method: "get",
            url: "/NhaXuatBan/Get_AllNhaXuatBan"
        }).then(function (response) {
            $scope.list = response.data;
            for (i = 0; i < response.data.length ; i++) {
                if (response.data[i].Id == id) {
                    $scope.IdNXB = response.data[i];
                }
            }
        }, function () {
            alert("Error Occur");
        })
    };
});

// nhập sách
app.controller('ImportBookCtrlr', function ($scope, $http) {

    $scope.list = [];
    $scope.addItem = function () {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "/PhieuNhapSach/_GetBookItemById",
            params: {
                maKS: $scope.maKS,
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
        }, function (e) {
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
            url: "/PhieuXuatSach/_GetBookItemById",
            params: {
                idBook: $scope.idBook,
                soLuong: $scope.soLuong,
                idTrangThai: $scope.idTrangThai,
                idLyDo: $scope.idLyDo
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
app.controller('MuonSachChooseBookCtrlr', function ($scope, $http) {
    $scope.list = [];

    $scope.addListItems = function (lstSach) {
        if (lstSach != null) {
            for (i = 0; i < lstSach.length; i++) {
                $scope.maKiemSoat = JSON.parse(lstSach[i]).MaKiemSoat;
                $scope.addItem();
            }
        }
    }

    $scope.addItem = function () {
        $scope.errortext = "";
        $scope.soLuongTemp = 1;
        $scope.found = false;
        $scope.list.forEach(function (item, idx) {
            if (item.MaKiemSoat == $scope.maKiemSoat) {
                $scope.found = true;
                $scope.soLuongTemp = item.SoLuongMuon + 1;
            }
        });

        $http({
            method: "get",
            url: "/PhieuMuon/_GetBookItemById",
            params: {
                MaKiemSoat: $scope.maKiemSoat,
                soLuong: $scope.soLuongTemp,
            }
        }).then(function (response) {
            if (response.data) {
                if (response.data.Status) {
                    if ($scope.found == true) {
                        $scope.list.forEach(function (item, idx) {
                            if (item.MaKiemSoat == $scope.maKiemSoat)
                                item.SoLuongMuon = response.data.SoLuongMuon
                        });
                    } else {
                        $scope.list.push(response.data);
                    }
                }
                else {
                    alert("Sách quá số lượng");
                }
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function () {
            alert("Error Occur");
        })
    }

    $scope.updateNumber = function (id) {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "/PhieuMuon/_GetBookItemById",
            params: {
                idBook: $scope.list[id].MaKiemSoat,
                soLuong: document.getElementById('sach' + id).value,
            }
        }).then(function (response) {
            if (response.data) {
                // cap nhat lai so luong
                if (response.data.Status != true) {
                    alert("Sách quá số lượng");
                }
                $scope.list[id].SoLuongMuon = response.data.SoLuongMuon;
                document.getElementById('sach' + id).value = response.data.SoLuongMuon;
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function () {
            alert("Error Occur");
        })
    };

    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };
});

// Get a book by Id - TraSachCtrlr  
app.controller('TraSachCtrlr', function ($scope, $http) {
    $scope.list = [];

    $scope.addItem = function (id) {
        var sltra = document.getElementById('sl' + id).value;
        var trangthai = document.getElementById('tt' + id).value;
        var idPM = document.getElementById('idPM').value;

        $http({
            method: "get",
            url: "/PhieuTra/GetThongTinPhieuTra",
            params: {
                idBook: id,
                soLuong: sltra,
                idTrangThai: trangthai,
                idPM: idPM
            }
        }).then(function (response) {
            if (response.data) {
                var found = false;
                $scope.list.forEach(function (item, idx) {
                    if (item.IdSach == id && item.IdTrangThaiSach == response.data.IdTrangThaiSach) {
                        item = response.data;
                        found = true;
                    }
                });

                if (found == false)
                    $scope.list.push(response.data);
            }
            else {
                alert("Dữ liệu không phù hợp");
            }
        }, function () {
            alert("Error Occur");
        })
    };

    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };

    //$scope.GetAllData = function () {
    //    $http({
    //        method: "get",
    //        url: "/PhieuMuon/GetChiTietPhieuJSon",
    //        params: {
    //            idPM: $('#idPM').val(),
    //            soLuong: 0
    //        }
    //    }).then(function (response) {
    //        $scope.listGet = response.data;
    //    }, function () {
    //        alert("Error Occur");
    //    })
    //};
});

app.controller('SachMuonCtrlr', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/PhieuMuon/GetChiTietPhieuJSon",
            params: {
                idPM: $('#idPM').val(),
                soLuong: 0
            }
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            alert("Error Occur");
        })
    };
});

// Define the `UserChucVuCtrlr` controller on the `LibraryApp` module
app.controller('UserChucVuCtrlr', function ($scope, $http) {

    $scope.GetAllData = function (id) {

        $http({
            method: "get",
            url: "/ChucVu/Get_AllChucVu"
        }).then(function (response) {
            $scope.list = response.data;
            for (i = 0; i < response.data.length ; i++) {
                if (response.data[i].Id == id) {
                    $scope.IdChucVu = response.data[i];
                }
            }
        }, function () {
            alert("Error Occur");
        })
    };

    $scope.GetNameChucVu = function (id) {
        $http({
            method: "POST",
            url: "/ChucVu/Get_Name",
            data: { id: id }
        }).then(function (response) {
            $scope.list = response.data;
            console.log($scope.list.TenChucVu)
            $scope.ChucVu = $scope.list.TenChucVu;
        }, function () {
            //alert("Error Occur");
        })
    };
});

