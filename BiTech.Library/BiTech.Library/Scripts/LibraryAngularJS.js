/// <reference path="angular.js" />
// Define the `LibraryApp` module

var app = angular.module('LibraryApp', []);

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
                idTrangThai: $scope.idTrangThai,
                GhiChu: $scope.GhiChu,
            }
        }).then(function (response) {
            if (response.data !== null) {
                $scope.list.push(response.data);
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function (e) {
            alert("bạn vui lòng điền vào ô trống");
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
                maKiemSoat: $scope.maKS,
                soLuong: $scope.soLuong,
                idTrangThai: $scope.idTrangThai,
                LyDo: $scope.LyDo,
                GhiChu:$scope.GhiChu
            }
        }).then(function (response) {
            if (response.data !== null) {
                $scope.list.push(response.data);
            }
            else {
                alert("Mã sách không phù hợp");
            }
        }, function () {
            alert("bạn vui lòng điền vào ô trống");
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
        if (lstSach !== null) {
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
            if (item.MaKiemSoat === $scope.maKiemSoat) {
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
                    if ($scope.found === true) {
                        $scope.list.forEach(function (item, idx) {
                            if (item.MaKiemSoat === $scope.maKiemSoat)
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
            alert("bạn vui lòng điền vào ô trống");
        })
    }

    $scope.updateNumber = function (id) {
        $scope.errortext = "";
        $http({
            method: "get",
            url: "/PhieuMuon/_GetBookItemById",
            params: {
                MaKiemSoat: $scope.list[id].MaKiemSoat,
                soLuong: document.getElementById('sach' + id).value,
            }
        }).then(function (response) {
            if (response.data) {
                // cap nhat lai so luong
                if (response.data.Status !== true) {
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
        //var id = document.getElementById('idSach').innerHTML; //id ma sach (id mongo)
        var sltra = document.getElementById('sl' + id).value;
        var trangthai = document.getElementById('tt' + id).value;
        var idPM = document.getElementById('idPM').value;

        $http({
            method: "get",
            url: "/PhieuTra/GetThongTinPhieuTra",
            params: {
                id: id,
                soLuong: sltra,
                idTrangThai: trangthai,
                idPM: idPM
            }
        }).then(function (response) {
            if (response.data) {
                var found = false;
                $scope.list.forEach(function (item, idx) {
                    if (item.IdSach === id && item.IdTrangThaiSach === response.data.IdTrangThaiSach) {
                        item = response.data;
                        found = true;
                    }
                });

                if (found === false)
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

app.controller('MuonSachCtrlr', function ($scope, $http, $filter, $location) {
    $scope.BoolThemSach = false; //Được phép thêm sách
    $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
    $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
    $scope.NoiDungThongBao = "Lỗi"; //Nội dung thông báo lỗi
    $scope.disabled_input = true; //disabled input
    $scope.loading_bar = false; //show loading bar 
    $scope.SoluongSachMuon = 1;
    $scope.list_book_queue = []; //list sách chuẩn bị cho mượn
    $scope.list_book_dangmuon = []; //list sách dang muon cho mượn
    
    //=====================Mượn Sách========================================================
    //load thông tin sách và ẩn loading bar
    $scope.GetBook_loading = function () {
        $scope.loading_bar = true;
        $scope.show_thongbao = false;
        setTimeout(function () {
            $scope.GetBook();
            $scope.loading_bar = false;
        }, 500);
    };

    //Load list sách user đang mượn - $http
    $scope.GetListBook = function () {
        var IdUser = parseLocation(window.location.search)['IdUser'];
        $http({
            method: "post",
            url: "/MuonSach/GetListBook_IdUser",
            params: {
                IdUser: IdUser
            }
        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;
            
        }, function () {
            alert("Error Occur");
        })
    };

    //Lấy thông tin sách qua masach - $http
    $scope.GetBook = function () {
        $http({
            method: "post",
            url: "/MuonSach/GetBook",
            params: {
                maSach: $("#MaSach").val()
            }
        }).then(function (response) {
            $scope.list = response.data;
            $scope.show_thongbao = true;
            if ($scope.list.length > 0) {
                $scope.masach = $scope.list[0].MaKiemSoat;
                $scope.TenSach = $scope.list[0].TenSach;
                $scope.SoLuong = (parseInt($scope.list[0].SoLuong) - parseInt($scope.Get_SoluongSachDangMuonTheoMKS($scope.masach))).toString();
                $scope.show_thongbao_CoSach = true;
                $scope.disabled_input = false;
                $scope.BoolThemSach = true;
                $scope.SoluongSachMuon = 1;
                load_datepicker();
            } else {
                $scope.show_thongbao_CoSach = false;
                $scope.disabled_input = true;
                $scope.BoolThemSach = false;
                $scope.NoiDungThongBao = ($scope.masach == "") ? "Chưa nhập mã sách" : "Mã sách không đúng"
            }
        }, function () {
            alert("Error Occur");
        })
    };

    //Mượn sách - $http
    $scope.MuonSach = function () {
        var DTO = JSON.stringify($scope.list_book_queue);
        //todo
        //update database
        $http({
            method: "post",
            url: "/MuonSach/UpdateListBook",
            contentType: 'application/json; charset=utf-8',
            data: DTO

        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;
            $scope.ResetListBookQueue();
        }, function () {
            alert("Error Occur");
        })
    }

    //Thêm sách vào danh sách sẽ mượn
    $scope.ThemSach = function () {
        if ($scope.SoLuong == 0) {
            $scope.BoolThemSach = false;
        }

        if (!$scope.BoolThemSach) {
            $scope.ResetBook();
            $scope.show_thongbao = true;
            $scope.show_thongbao_CoSach = false;
            $scope.NoiDungThongBao = ($scope.SoLuong == 0) ? "Số lượng sách đã hết" : ($scope.masach == "") ? "Bạn chưa nhập mã sách" : "lỗi thêm sách";
        } else {

            //Kiểm tra MaKiemSoat có tồn tại trong list chưa
            let index = $scope.list_book_queue.findIndex(_ => _.MaKiemSoat == $scope.masach);
            if (index >= 0) {
                //Đã tồn tại
                $scope.list_book_queue[index].SoLuong = $scope.list_book_queue[index].SoLuong + $scope.SoluongSachMuon;
            } else {
                //Chưa tồn tại
                $scope.items = {
                    Id: "",
                    IdUser: parseLocation(window.location.search)['IdUser'],
                    MaKiemSoat: $scope.masach,
                    TenSach: $scope.TenSach,
                    SoLuong: $scope.SoluongSachMuon,
                    NgayMuon: Date.now().toString(),
                    NgayTra: $("#ngayPhaiTra").val(),
                    TinhTrang: false
                };
                $scope.list_book_queue.push($scope.items);
            }
            $scope.ResetBook();
        }
    };

    //Event nhấp enter trên input[type="text"] nhập masach
    $scope.myFunct = function (keyEvent) {
        if (keyEvent.keyCode == 13) {
            $scope.GetBook_loading();
        }
    };

    //=====================End Mượn Sách========================================================

    //load focus input số lượng sách có thể mượn trong giới hạn min max
    $scope.loadfocus_input_soluongsachmuon = function () {
        var Soluong = $scope.SoLuong;
        if ($scope.SoluongSachMuon == null) {
            $scope.SoluongSachMuon = parseInt(Soluong);
        } else if ($scope.SoluongSachMuon == 0) {
            $scope.SoluongSachMuon = 1;
        }
    };

    //reset form thông tin sách
    $scope.ResetBook = function () {
        $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
        $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
        $scope.disabled_input = true; //disabled input
        $scope.loading_bar = false; //show loading bar 
        $scope.BoolThemSach = false;
        $scope.masach = "";
        $scope.TenSach = "";
        $scope.SoluongSachMuon = 1;
        load_datepicker();
    };

    //xoá item khỏi list_book_queue
    $scope.removeItem = function (x) {
        $scope.list_book_queue = $filter('orderBy')($scope.list_book_queue, '-NgayMuon');
        $scope.list_book_queue.splice(x, 1);
        $scope.ResetBook();
    }

    //Clear list book queue
    $scope.ResetListBookQueue = function () {
        $scope.list_book_queue = []
    };

    //Lấy số lượng sách đang mượn
    $scope.Get_SoluongSachDangMuonTheoMKS = function (mks) {
        let index = $scope.list_book_queue.findIndex(_ => _.MaKiemSoat == mks);
        if (index >= 0) {
            return $scope.list_book_queue[index].SoLuong;
        }
        return 0;
    };

    //lấy parameter url
    var parseLocation = function (location) {
        var pairs = location.substring(1).split("&");
        var obj = {};
        var pair;
        var i;

        for (i in pairs) {
            if (pairs[i] === "") continue;

            pair = pairs[i].split("=");
            obj[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
        }

        return obj;
    };
    
});

app.controller('TraSachCtrlr', function ($scope, $http, $filter, $location) {
    $scope.BoolThemSach = false; //Được phép thêm sách
    $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
    $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
    $scope.NoiDungThongBao = "Lỗi"; //Nội dung thông báo lỗi
    $scope.disabled_input = true; //disabled input
    $scope.loading_bar = false; //show loading bar 
    $scope.SoluongSachMuon = 1;
    $scope.list_book_queue = []; //list sách chuẩn bị cho mượn
    $scope.list_book_dangmuon = []; //list sách dang muon cho mượn

    //=====================Trả Sách========================================================

    //Load list sách user đang mượn - $http
    $scope.GetListBook = function () {
        var IdUser = parseLocation(window.location.search)['IdUser'];
        $http({
            method: "post",
            url: "/TraSach/GetListBook_IdUser",
            params: {
                IdUser: IdUser
            }
        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;

        }, function () {
            alert("Error Occur");
        })
    };

    //Lấy thông tin sách qua masach - Trả sách - $http
    $scope.GetBook_TraSach = function () {
        $http({
            method: "post",
            url: "/TraSach/GetBook_TraSach",
            params: {
                maSach: $("#MaSach").val(),
                IdUser: parseLocation(window.location.search)['IdUser']
            }
        }).then(function (response) {
            $scope.list = response.data;
            $scope.show_thongbao = true;
            if ($scope.list.length > 0) {
                $scope.masach = $scope.list[0].MaKiemSoat;
                $scope.TenSach = $scope.list[0].TenSach;
                $scope.SoLuong = (parseInt($scope.list[0].SoLuong) - parseInt($scope.Get_SoluongSachDangMuonTheoMKS($scope.masach))).toString();
                $scope.NgayMuon = $scope.list[0].NgayMuon;
                $scope.show_thongbao_CoSach = true;
                $scope.disabled_input = false;
                $scope.BoolThemSach = true;
                $scope.SoluongSachMuon = 1;
                load_datepicker();
            } else {
                $scope.show_thongbao_CoSach = false;
                $scope.disabled_input = true;
                $scope.BoolThemSach = false;
                $scope.NoiDungThongBao = ($scope.masach == "") ? "Chưa nhập mã sách" : "Mã sách hiện không có trong danh sách mượn của bạn"
            }
        }, function () {
            alert("Error Occur");
        })
    };

    //Trả sách - $http
    $scope.TraSach = function () {
        var DTO = JSON.stringify($scope.list_book_queue);
        //todo
        //update database
        $http({
            method: "post",
            url: "/TraSach/UpdateListBook",
            contentType: 'application/json; charset=utf-8',
            data: DTO

        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;
            $scope.ResetListBookQueue();
        }, function () {
            alert("Error Occur");
        })
    }

    //Thêm sách vào danh sách sẽ mượn
    $scope.ThemSach_TraSach = function () {
        if ($scope.SoLuong == 0) {
            $scope.BoolThemSach = false;
        }

        if (!$scope.BoolThemSach) {
            $scope.ResetBook();
            $scope.show_thongbao = true;
            $scope.show_thongbao_CoSach = false;
            $scope.NoiDungThongBao = ($scope.SoLuong == 0) ? "Số lượng sách đã hết" : ($scope.masach == "") ? "Bạn chưa nhập mã sách" : "lỗi thêm sách";
        } else {

            //Kiểm tra MaKiemSoat có tồn tại trong list chưa
            let index = $scope.list_book_queue.findIndex(_ => _.MaKiemSoat == $scope.masach);
            if (index >= 0) {
                //Đã tồn tại
                $scope.list_book_queue[index].SoLuong = $scope.list_book_queue[index].SoLuong + $scope.SoluongSachMuon;
            } else {
                //Chưa tồn tại
                $scope.items = {
                    Id: "",
                    IdUser: parseLocation(window.location.search)['IdUser'],
                    MaKiemSoat: $scope.masach,
                    TenSach: $scope.TenSach,
                    SoLuong: $scope.SoluongSachMuon,
                    NgayMuon: $scope.NgayMuon,
                    NgayTra: Date.now().toString(),
                    TinhTrang: false
                };
                $scope.list_book_queue.push($scope.items);
            }
            $scope.ResetBook();
        }
    };

    //Event nhấp enter trên input[type="text"] nhập masach
    $scope.myFunct_TraSach = function (keyEvent) {
        if (keyEvent.keyCode == 13) {
            $scope.GetBook_TraSach_loading();
        }
    };

    //load thông tin sách và ẩn loading bar
    $scope.GetBook_TraSach_loading = function () {
        $scope.loading_bar = true;
        $scope.show_thongbao = false;
        setTimeout(function () {
            $scope.GetBook_TraSach();
            $scope.loading_bar = false;
        }, 500);
    };

    //=====================End Trả Sách========================================================

    //load focus input số lượng sách có thể mượn trong giới hạn min max
    $scope.loadfocus_input_soluongsachmuon = function () {
        var Soluong = $scope.SoLuong;
        if ($scope.SoluongSachMuon == null) {
            $scope.SoluongSachMuon = parseInt(Soluong);
        } else if ($scope.SoluongSachMuon == 0) {
            $scope.SoluongSachMuon = 1;
        }
    };

    //reset form thông tin sách
    $scope.ResetBook = function () {
        $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
        $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
        $scope.disabled_input = true; //disabled input
        $scope.loading_bar = false; //show loading bar 
        $scope.BoolThemSach = false;
        $scope.masach = "";
        $scope.TenSach = "";
        $scope.SoluongSachMuon = 1;
        load_datepicker();
    };

    //xoá item khỏi list_book_queue
    $scope.removeItem = function (x) {
        $scope.list_book_queue = $filter('orderBy')($scope.list_book_queue, '-NgayMuon');
        $scope.list_book_queue.splice(x, 1);
        $scope.ResetBook();
    }

    //Clear list book queue
    $scope.ResetListBookQueue = function () {
        $scope.list_book_queue = []
    };

    //Lấy số lượng sách đang mượn
    $scope.Get_SoluongSachDangMuonTheoMKS = function (mks) {
        let index = $scope.list_book_queue.findIndex(_ => _.MaKiemSoat == mks);
        if (index >= 0) {
            return $scope.list_book_queue[index].SoLuong;
        }
        return 0;
    };

    //lấy parameter url
    var parseLocation = function (location) {
        var pairs = location.substring(1).split("&");
        var obj = {};
        var pair;
        var i;

        for (i in pairs) {
            if (pairs[i] === "") continue;

            pair = pairs[i].split("=");
            obj[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
        }

        return obj;
    };
});