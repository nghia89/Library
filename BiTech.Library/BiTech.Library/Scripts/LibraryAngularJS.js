﻿/// <reference path="angular.js" />
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
            for (i = 0; i < response.data.length; i++) {
                if (response.data[i].Id == id) {
                    $scope.IdTheLoai = response.data[i];
                }
            }
        }, function () {
            console.log("BookGenresCtrlr - GetAllData - load fail.");
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
            for (i = 0; i < response.data.length; i++) {
                if (response.data[i].Id == id) {
                    $scope.IdNhaXuatBan = response.data[i];
                }
            }
        }, function () {
            console.log("PublishersCtrlr - GetAllData - load fail.");
        })
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
            console.log("MuonSachChooseBookCtrlr - updateNumber - update fail.");
        })
    };

    $scope.removeItem = function (x) {
        $scope.errortext = "";
        $scope.list.splice(x, 1);
    };
});

// Get a book by Id - TraSachCtrlr  
/*
HEAD

app.controller('TraSachCtrlr', function ($scope, $http) {

app.controller('TraSachCtrlr_Vinh', function ($scope, $http) {

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
            console.log("");
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
    //        console.log("");
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
            console.log("");
        })
    };
});
*/

app.controller('KeSach', function ($scope, $http) {

    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "/KeSach/GetAll"
        }).then(function (response) {
            $scope.list = response.data;
        }, function () {
            console.log("KeSach - GetAllData - load fail.");
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
            console.log("NgonNgu - GetAllData - laod fail.");
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
            console.log("TacGiaSelectorCtrlr - GetAllData - load fail.");
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
    $scope.kqmuon = false; //Kết quả mượn là true thì show thanh search và kết quả mượn

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
            $scope.UpdateListChuanBiMuon(); //update lại list_book_queue == null cho controler khi loaded user mới
        }, function () {
            console.log("MuonSachCtrlr - GetListBook - load fail.");
        })
    };

    //Mượn sách - $http
    $scope.MuonSach = function () {
        var IdUser = parseLocation(window.location.search)['IdUser'];
        IdUser = $scope.GetInfoIdUser(IdUser);
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
            if ($scope.list_book_dangmuon.length > 0) {
                $scope.UpdateListChuanBiMuon();
                window.location = '/MuonSach?IdUser=' + IdUser + '&flagResult=true';
            }
            $scope.ResetListBookQueue();

        }, function () {
            console.log("MuonSachCtrlr - MuonSach - fail.");
        })
    }

    //Event nhấp enter trên input[type="text"] nhập masach
    $scope.myFunct = function (keyEvent) {
        if (keyEvent.keyCode == 13) {
            $scope.GetBook_loading();
        }
    };

    //lấy sách thêm vào list
    $scope.GetBook = function () {
        $scope.masach = $scope.GetInfoSach($("#MaSach").val());
        $http({
            method: "post",
            url: "/MuonSach/GetBook",
            params: {
                maSach: $("#MaSach").val()
            }
        }).then(function (response) {
            $scope.list = response.data;
            var bool_kq = false;
            //Kiểm tra
            if ($scope.list.length > 0) {
                bool_kq = true;
            } else {
                bool_kq = false;
                $scope.NoiDungThongBao = ($scope.masach == "") ? "Chưa nhập mã sách" : "Sách này hiện không có trong kho";
            }

            //kết quả
            if (bool_kq) {
                //Chưa tồn tại
                $scope.items = {
                    Id: "",
                    IdUser: parseLocation(window.location.search)['IdUser'],
                    MaKiemSoat: $scope.list[0].MaKiemSoat,
                    MaKSCB: $scope.list[0].MaKSCB,
                    TenSach: $scope.list[0].TenSach,
                    NgayMuon: Date.now().toString(),
                    NgayTra: $("#ngayPhaiTra").val(),
                    TinhTrang: false
                };
                $scope.list_book_queue.push($scope.items);
            } else {
                $scope.show_thongbao = true;
            }
            $scope.UpdateListChuanBiMuon();
            $scope.masach = "";
        }, function () {
            console.log("MuonSachCtrlr - GetBook - fail.");
        })
    };

    //update list chuẩn bị mượn tới controller
    $scope.UpdateListChuanBiMuon = function () {
        var DTO = JSON.stringify($scope.list_book_queue);
        $http({
            method: "post",
            url: "/MuonSach/UpdateList_ChuanBiMuon",
            contentType: 'application/json; charset=utf-8',
            data: DTO
        }).then(function (response) {
            return true;
        }, function () {
            console.log("TraSachCtrlr - UpdateListChuanBiTra - update fail.");
            return false;
        })
    }

    //=====================End Mượn Sách========================================================

    //Lấy mã sách bằng nhập tay và quét mã QR
    $scope.GetInfoSach = function (masach) {
        try {
            var arrStr = masach.split('-');
            var MaKiemSoat = masach;
            if (arrStr[0] == "BLibBook") {
                var id = arrStr[1];
                MaKiemSoat = arrStr[2];
                var tenSach = arrStr[3];
            }
            return MaKiemSoat;
        }
        catch (err) {
            return masach;
        }
    }

    $scope.GetInfoIdUser = function (masach) {
        try {
            var arrStr = masach.split('-');
            var MaKiemSoat = masach;
            if (arrStr[0] == "BLibUser") {
                var id = arrStr[1];
                MaKiemSoat = arrStr[2];
                var tenSach = arrStr[3];
            }
            return MaKiemSoat;
        }
        catch (err) {
            return masach;
        }
    }

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
        $scope.list_book_queue = [];
        $scope.ResetBook();
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

}).directive('myRepeatDirective', function ($timeout) {
    return function (scope, element, attrs) {
        //Load function sau khi run ng-repeat
        $timeout(function () {
            try {
                load_datepicker("ngayPhaiTra_" + scope.x['MaKiemSoat']);

                //update ngày trả sách trong list chuẩn bị cho mượn
                $("#ngayPhaiTra_" + scope.x['MaKiemSoat']).change(function () {
                    scope.x['NgayTra'] = $("#ngayPhaiTra_" + scope.x['MaKiemSoat']).val()
                });
            } catch (exp) { }
        });
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
    $scope.list_book_queue = []; //list sách chuẩn bị trả
    $scope.list_book_dangmuon = []; //list sách user đang mượn
    $scope.list_book_TinhTrang = []; //List danh sách tình trạng trả
    $scope.ChoiceInlistbookdangmuon_MaSach = "";
    $scope.ChoiceInlistbookdangmuon_NgayMuon = "";
    $scope.ChoiceInlistbookdangmuon_NgayTra = "";

    //=====================Trả Sách========================================================
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
            url: "/TraSach/GetListBook_IdUser",
            params: {
                IdUser: IdUser
            }
        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;
            $scope.UpdateListChuanBiTra(); //update lại list_book_queue == null cho controler khi loaded user mới
        }, function () {
            console.log("TraSachCtrlr - GetListBook - fail.");
        })
    };

    //Trả sách - $http
    $scope.TraSach = function () {
        var IdUser = parseLocation(window.location.search)['IdUser'];
        IdUser = $scope.GetInfoIdUser(IdUser);
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
            if ($scope.list_book_dangmuon.length > 0) {
                $scope.UpdateListChuanBiTra();
                window.location = '/TraSach?IdUser=' + IdUser + '&flagResult=true';
            }
            if ($scope.list_book_dangmuon.length == 0 && $scope.list_book_queue.length > 0) {
                $scope.UpdateListChuanBiTra();
                window.location = '/TraSach?IdUser=' + IdUser + '&flagResult=true';
            }
            $scope.ResetListBookQueue();

        }, function () {
            console.log("TraSachCtrlr - TraSach - fail.");
        })
        $scope.ResetBook();
    }

    //Event nhấp enter trên input[type="text"] nhập masach
    $scope.myFunct = function (keyEvent) {
        if (keyEvent.keyCode == 13) {
            $scope.GetBook_loading();
        }
    };

    //lấy sách thêm vào list
    $scope.GetBook = function () {
        $scope.UpdateListChuanBiTra();
        $http({
            method: "post",
            url: "/TraSach/GetBook",
            params: {
                maSach: $("#MaSach").val(),
                IdUser: parseLocation(window.location.search)['IdUser']
            }

        }).then(function (response) {
            $scope.GetBook_Result(response);
        }, function () {
            console.log("TraSachCtrlr - GetBook - fail.");
        })
        $scope.UpdateListChuanBiTra();
    };

    //lấy sách thêm vào list (2) chọn trên danh sách
    $scope.GetBook_2 = function (MaSach, NgayMuon, NgayTra) {
        $scope.UpdateListChuanBiTra();
        MaSach = $scope.GetInfoSach(MaSach);
        $scope.show_thongbao = false;
        $http({
            method: "post",
            url: "/TraSach/GetBook",
            params: {
                maSach: MaSach,
                IdUser: parseLocation(window.location.search)['IdUser'],
                NgayMuon: NgayMuon,
                NgayTra: NgayTra
            }

        }).then(function (response) {
            $scope.GetBook_Result(response);
        }, function () {
            console.log("TraSachCtrlr - GetBook_2 - load fail.");
        })
        $scope.UpdateListChuanBiTra();
    };

    //lấy sách thêm vào list (2) chọn trên danh sách - show model chọn tình trạng
    $scope.GetBook_Choice_TinhTrang = function (MaSach, NgayMuon, NgayTra) {
        $scope.ChoiceInlistbookdangmuon_MaSach = MaSach;
        $scope.ChoiceInlistbookdangmuon_NgayMuon = NgayMuon;
        $scope.ChoiceInlistbookdangmuon_NgayTra = NgayTra;
        show_modal();
    }

    //lấy sách thêm vào list (2) chọn trên danh sách - show model chọn tình trạng - result true
    $scope.GetBook_Choice_TinhTrang_result_true = function () {
        $scope.GetBook_2($scope.ChoiceInlistbookdangmuon_MaSach, $scope.ChoiceInlistbookdangmuon_NgayMuon, $scope.ChoiceInlistbookdangmuon_NgayTra);
        hide_modal();
    }

    //$scope.GetBook True
    $scope.GetBook_Result = function (response) {
        $scope.list = response.data;
        var bool_kq = false;
        //Kiểm tra
        if ($scope.list.length > 0) {
            bool_kq = true;
        } else {
            //Mã sách không tồn tại trong danh sách đang mượn
            //or đối tượng được chọn đã tồn tại trên list chuẩn bị trả
            bool_kq = false;
            $scope.NoiDungThongBao = "Sách này hiện đã chọn hoặc không tồn tại trong danh sách đang mượn";
        }

        //kết quả
        if (bool_kq) {
            //Chưa tồn tại
            var edit_NgayMuon = $scope.list[0].NgayMuon.split('/').join('-'); //format lại date
            var edit_Ngaytra = $scope.list[0].NgayTra.split('/').join('-'); //format lại date
            var index_TT = $scope.list_book_TinhTrang.findIndex(_ => _.Id == $scope.modelTT); //lấy thông tin của tình trạng đang chọn
            $scope.items = {
                Id: $scope.list[0].MaKiemSoat + edit_NgayMuon + edit_Ngaytra + $scope.modelTT,
                IdUser: parseLocation(window.location.search)['IdUser'],
                MaKiemSoat: $scope.list[0].MaKiemSoat,
                MaKSCB: $scope.list[0].MaKSCB,
                TenSach: $scope.list[0].TenSach,
                SoLuong: "1",
                SoLuongMax: (parseInt($scope.list[0].SoLuong)).toString(),
                NgayMuon: $scope.list[0].NgayMuon,
                NgayTra: $scope.list[0].NgayTra,
                TinhTrangSach: $scope.modelTT,
                TinhTrangSachTen: $scope.list_book_TinhTrang[index_TT].TenTT,
                TinhTrang: false
            };
            $scope.list_book_queue.push($scope.items);
        } else {
            $scope.show_thongbao = true;
        }
        $scope.UpdateListChuanBiTra();
        $scope.GetAllTrangThaiSach();
        $scope.masach = "";
    }

    //update list chuẩn bị trả tới controller
    $scope.UpdateListChuanBiTra = function () {
        var DTO = JSON.stringify($scope.list_book_queue);
        $http({
            method: "post",
            url: "/TraSach/UpdateList_ChuanBiTra",
            contentType: 'application/json; charset=utf-8',
            data: DTO
        }).then(function (response) {
            return true;
        }, function () {
            console.log("TraSachCtrlr - UpdateListChuanBiTra - update fail.");
            return false;
        })
    }

    //=====================End Trả Sách========================================================

    //Lấy mã sách bằng nhập tay và quét mã QR
    $scope.GetInfoSach = function (masach) {
        try {
            var arrStr = masach.split('-');
            var MaKiemSoat = masach;
            if (arrStr[0] == "BLibBook") {
                var id = arrStr[1];
                MaKiemSoat = arrStr[2];
                var tenSach = arrStr[3];
            }
            return MaKiemSoat;
        }
        catch (err) {
            return masach;
        }
    }

    $scope.GetInfoIdUser = function (masach) {
        try {
            var arrStr = masach.split('-');
            var MaKiemSoat = masach;
            if (arrStr[0] == "BLibUser") {
                var id = arrStr[1];
                MaKiemSoat = arrStr[2];
                var tenSach = arrStr[3];
            }
            return MaKiemSoat;
        }
        catch (err) {
            return masach;
        }
    }

    $scope.GetAllTrangThaiSach = function () {
        $http({
            method: "post",
            url: "/TraSach/GetAllTrangThaiSach",
        }).then(function (response) {
            $scope.list_book_TinhTrang = response.data;
            if ($scope.list_book_TinhTrang.length > 0) {
                $scope.modelTT = $scope.list_book_TinhTrang[0].Id;
            }

        }, function () {
            console.log("TraSachCtrlr - GetAllTrangThaiSach - load fail.");
            return false;
        })
    }

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
        $scope.GetAllTrangThaiSach();
    };

    //xoá item khỏi list_book_queue
    $scope.removeItem = function (x) {
        $scope.list_book_queue = $filter('orderBy')($scope.list_book_queue, '-NgayMuon');
        $scope.list_book_queue.splice(x, 1);
        $scope.ResetBook();
    }

    //Clear list book queue
    $scope.ResetListBookQueue = function () {
        $scope.list_book_queue = [];
        $scope.ResetBook();
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

    //init
    $scope.GetAllTrangThaiSach();

}).directive('myRepeatDirective', function ($timeout) {
    return function (scope, element, attrs) {
        //Load function sau khi run ng-repeat
        $timeout(function () {
            try {
                load_datepicker("ngayPhaiTra_" + scope.x['MaKiemSoat']);

                //update số lượng sách trong list chuẩn bị cho mượn
                $("#List_" + scope.x['Id']).change(function () {
                    scope.x['SoLuong'] = $("#List_" + scope.x['Id']).val()
                });

                //update ngày trả sách trong list chuẩn bị cho mượn
                $("#ngayPhaiTra_" + scope.x['MaKiemSoat']).change(function () {
                    scope.x['NgayTra'] = $("#ngayPhaiTra_" + scope.x['MaKiemSoat']).val()
                });

                //update ngày trả sách trong list chuẩn bị cho mượn
                $("#Select_" + scope.x['Id']).change(function () {
                    scope.x['TinhTrangSach'] = $("#Select_" + scope.x['Id']).val()
                });
            } catch (exp) { }
        });
    };
});

app.controller('GiaHanCtrlr', function ($scope, $http, $filter, $location) {
    $scope.BoolThemSach = false; //Được phép thêm sách
    $scope.show_thongbao = false; //Hiện thị div thông báo (chứa thông tin sách or báo lỗi)
    $scope.show_thongbao_CoSach = false; //True: hiện thông tin sách; false: hiện báo lỗi
    $scope.NoiDungThongBao = "Lỗi"; //Nội dung thông báo lỗi
    $scope.disabled_input = true; //disabled input
    $scope.loading_bar = false; //show loading bar 
    $scope.SoluongSachMuon = 1;
    $scope.list_book_queue = []; //list sách chuẩn bị trả
    $scope.list_book_dangmuon = []; //list sách user đang mượn
    $scope.list_book_TinhTrang = []; //List danh sách tình trạng trả
    $scope.ChoiceInlistbookdangmuon_MaSach = "";
    $scope.ChoiceInlistbookdangmuon_NgayMuon = "";
    $scope.ChoiceInlistbookdangmuon_NgayTra = "";

    //=====================gia han========================================================
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
            url: "/GiaHan/GetListBook_IdUser",
            params: {
                IdUser: IdUser
            }
        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;
            $scope.UpdateListChuanBiTra(); //update lại list_book_queue == null cho controler khi loaded user mới
        }, function () {
            console.log("GiaHanCtrlr - GetListBook - fail.");
        })
    };

    //Gia hạn - $http
    $scope.GiaHan = function () {
        var IdUser = parseLocation(window.location.search)['IdUser'];
        IdUser = $scope.GetInfoIdUser(IdUser);
        var DTO = JSON.stringify($scope.list_book_queue);
        //todo
        //update database
        $http({
            method: "post",
            url: "/GiaHan/UpdateListBook",
            contentType: 'application/json; charset=utf-8',
            data: DTO

        }).then(function (response) {
            $scope.list_book_dangmuon = response.data;
            if ($scope.list_book_dangmuon.length > 0) {
                $scope.UpdateListChuanBiTra();
                window.location = '/GiaHan?IdUser=' + IdUser + '&flagResult=true';
            }
            if ($scope.list_book_dangmuon.length == 0 && $scope.list_book_queue.length > 0) {
                $scope.UpdateListChuanBiTra();
                window.location = '/GiaHan?IdUser=' + IdUser + '&flagResult=true';
            }
            $scope.ResetListBookQueue();

        }, function () {
            console.log("GiaHanCtrlr - GiaHan - fail.");
        })
        $scope.ResetBook();
    }

    //Event nhấp enter trên input[type="text"] nhập masach
    $scope.myFunct = function (keyEvent) {
        if (keyEvent.keyCode == 13) {
            $scope.GetBook_loading();
        }
    };

    //lấy sách thêm vào list
    $scope.GetBook = function () {
        $scope.UpdateListChuanBiTra();
        $http({
            method: "post",
            url: "/GiaHan/GetBook",
            params: {
                maSach: $("#MaSach").val(),
                IdUser: parseLocation(window.location.search)['IdUser']
            }

        }).then(function (response) {
            $scope.GetBook_Result(response);
        }, function () {
            console.log("GiaHanCtrlr - GetBook - fail.");
        })
        $scope.UpdateListChuanBiTra();
    };

    //lấy sách thêm vào list (2) chọn trên danh sách
    $scope.GetBook_2 = function (MaSach, NgayMuon, NgayTra) {
        $scope.UpdateListChuanBiTra();
        MaSach = $scope.GetInfoSach(MaSach);
        $scope.show_thongbao = false;
        $http({
            method: "post",
            url: "/GiaHan/GetBook",
            params: {
                maSach: MaSach,
                IdUser: parseLocation(window.location.search)['IdUser'],
                NgayMuon: NgayMuon,
                NgayTra: NgayTra
            }

        }).then(function (response) {
            $scope.GetBook_Result(response);
        }, function () {
            console.log("GiaHanCtrlr - GetBook_2 - load fail.");
        })
        $scope.UpdateListChuanBiTra();
    };

    //lấy sách thêm vào list (2) chọn trên danh sách - show model chọn tình trạng
    $scope.GetBook_Choice_DayNew = function (MaSach, NgayMuon, NgayTra) {
        $scope.ChoiceInlistbookdangmuon_MaSach = MaSach;
        $scope.ChoiceInlistbookdangmuon_NgayMuon = NgayMuon;
        $scope.ChoiceInlistbookdangmuon_NgayTra = NgayTra;

        load_datepicker("ngayPhaiTra_Change", NgayTra);
        show_modal();
        $(".ngayPhaiTra_Change").data("chon", true);
    }

    //lấy sách thêm vào list (2) chọn trên danh sách - show model chọn tình trạng - result true
    $scope.GetBook_Choice_TinhTrang_result_true = function () {
        $scope.GetBook_2($scope.ChoiceInlistbookdangmuon_MaSach, $scope.ChoiceInlistbookdangmuon_NgayMuon, $scope.ChoiceInlistbookdangmuon_NgayTra);

        hide_modal();
    }

    //$scope.GetBook True
    $scope.GetBook_Result = function (response) {
        $scope.list = response.data;
        var bool_kq = false;
        //Kiểm tra
        if ($scope.list.length > 0) {
            bool_kq = true;
        } else {
            //Mã sách không tồn tại trong danh sách đang mượn
            //or đối tượng được chọn đã tồn tại trên list chuẩn bị trả
            bool_kq = false;
            $scope.NoiDungThongBao = "Sách đã được chọn hoặc không tồn tại trong danh sách mượn";
        }

        //kết quả
        if (bool_kq) {

            //Chưa tồn tại
            var edit_NgayMuon = $scope.list[0].NgayMuon.split('/').join('-'); //format lại date
            var edit_Ngaytra = $scope.list[0].NgayTra.split('/').join('-'); //format lại date
            var index_TT = $scope.list_book_TinhTrang.findIndex(_ => _.Id == $scope.modelTT); //lấy thông tin của tình trạng đang chọn
            $scope.items = {
                Id: $scope.list[0].MaKiemSoat + edit_NgayMuon + edit_Ngaytra,
                IdUser: parseLocation(window.location.search)['IdUser'],
                MaKiemSoat: $scope.list[0].MaKiemSoat,
                MaKSCB: $scope.list[0].MaKSCB,
                TenSach: $scope.list[0].TenSach,
                SoLuong: $scope.list[0].SoLuong,
                SoLuongMax: (parseInt($scope.list[0].SoLuong)).toString(),
                NgayMuon: $scope.list[0].NgayMuon,
                NgayTra: $scope.list[0].NgayTra,
                NgayTraNew: $scope.list[0].NgayTra,
                TinhTrangSach: $scope.modelTT,
                TinhTrangSachTen: $scope.list_book_TinhTrang[index_TT].TenTT,
                TinhTrang: false
            };
            $scope.list_book_queue.push($scope.items);
        } else {
            $scope.show_thongbao = true;
        }
        $scope.UpdateListChuanBiTra();
        $scope.GetAllTrangThaiSach();
        $scope.masach = "";
    }

    //update list chuẩn bị trả tới controller
    $scope.UpdateListChuanBiTra = function () {
        var DTO = JSON.stringify($scope.list_book_queue);
        $http({
            method: "post",
            url: "/GiaHan/UpdateList_ChuanBiTra",
            contentType: 'application/json; charset=utf-8',
            data: DTO
        }).then(function (response) {
            return true;
        }, function () {
            console.log("GiaHanCtrlr - UpdateListChuanBiTra - update fail.");
            return false;
        })
    }

    //=====================End gia han========================================================

    //Lấy mã sách bằng nhập tay và quét mã QR
    $scope.GetInfoSach = function (masach) {
        try {
            var arrStr = masach.split('-');
            var MaKiemSoat = masach;
            if (arrStr[0] == "BLibBook") {
                var id = arrStr[1];
                MaKiemSoat = arrStr[2];
                var tenSach = arrStr[3];
            }
            return MaKiemSoat;
        }
        catch (err) {
            return masach;
        }
    }

    $scope.GetInfoIdUser = function (masach) {
        try {
            var arrStr = masach.split('-');
            var MaKiemSoat = masach;
            if (arrStr[0] == "BLibUser") {
                var id = arrStr[1];
                MaKiemSoat = arrStr[2];
                var tenSach = arrStr[3];
            }
            return MaKiemSoat;
        }
        catch (err) {
            return masach;
        }
    }

    $scope.GetAllTrangThaiSach = function () {
        $http({
            method: "post",
            url: "/GiaHan/GetAllTrangThaiSach",
        }).then(function (response) {
            $scope.list_book_TinhTrang = response.data;
            if ($scope.list_book_TinhTrang.length > 0) {
                $scope.modelTT = $scope.list_book_TinhTrang[0].Id;
            }

        }, function () {
            console.log("GiaHanCtrlr - GetAllTrangThaiSach - load fail.");
            return false;
        })
    }

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
        $scope.GetAllTrangThaiSach();
    };

    //xoá item khỏi list_book_queue
    $scope.removeItem = function (x) {
        $scope.list_book_queue = $filter('orderBy')($scope.list_book_queue, '-NgayMuon');
        $scope.list_book_queue.splice(x, 1);
        $scope.ResetBook();
    }

    //Clear list book queue
    $scope.ResetListBookQueue = function () {
        $scope.list_book_queue = [];
        $scope.ResetBook();
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

    //init
    $scope.GetAllTrangThaiSach();

}).directive('myRepeatDirective', function ($timeout) {
    return function (scope, element, attrs) {
        //Load function sau khi run ng-repeat
        $timeout(function () {
            try {
                //Nếu typselect = true là chọn từ danh sách đang mượn thì lấy kết quả tư .ngayPhaiTra_Change
                //Nều typselect = false nhập từ input text thì lấy kết quả ngày trả trong list đang mượn
                var typselect = $(".ngayPhaiTra_Change").data("chon");
                if (typselect) {
                    load_datepicker("ngayPhaiTra_" + scope.x['Id'], $(".ngayPhaiTra_Change").val());
                    scope.x['NgayTraNew'] = $(".ngayPhaiTra_Change").val();
                    $(".ngayPhaiTra_Change").data("chon", false);
                } else {
                    load_datepicker("ngayPhaiTra_" + scope.x['Id'], scope.x['NgayTra']);
                }

                //update số lượng sách trong list chuẩn bị cho mượn
                $("#List_" + scope.x['Id']).change(function () {
                    scope.x['SoLuong'] = $("#List_" + scope.x['Id']).val()
                });

                //update ngày trả sách trong list chuẩn bị cho mượn
                $(".ngayPhaiTra_" + scope.x['Id']).change(function () {
                    scope.x['NgayTraNew'] = $(".ngayPhaiTra_" + scope.x['Id']).val()
                });

                //update ngày trả sách trong list chuẩn bị cho mượn
                $("#Select_" + scope.x['Id']).change(function () {
                    scope.x['TinhTrangSach'] = $("#Select_" + scope.x['Id']).val()
                });
            } catch (exp) { }
        });
    };
});

//======================================Thống Kê=======================================
app.controller('ThongKeCtrlr', function ($scope, $http) {
    $scope.KeyMonth = '';
    $scope.KeyYear = '';
    function GetMonth() {
        $http({
            method: "post",
            url: "/ThongKe/MuonSachJson",
            params: {
                month: $scope.KeyMonth,
                year: $scope.KeyYear
            }
        }).then(function (response) {
            if (response.data !== null) {
                $scope.ListMonthMuonSach = response.data;
                DemSoNguoiMuonSach();
            }
        })
    }
    // Xử lý khi chọn Tháng/Năm
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    $scope.SelectedMonth = month.toString();
    $scope.SelectedYear = year.toString();
    $scope.GetDataMonth = function () {
        key1 = $scope.SelectedMonth;
        $scope.KeyMonth = key1;

        key2 = $scope.SelectedYear;
        $scope.KeyYear = key2;
        GetMonth();
    }
    GetMonth();
    /*===============================*/
    $scope.Day = '';
    function GetDay() {
        $http({
            method: "post",
            url: "/ThongKe/MuonSachJson",
            params: {
                day: $scope.Day,
            }
        }).then(function (response) {
            if (response != null) {
                $scope.ListDayMuonSach = response.data;
                DemSoNguoiMuonSach();
            }
        })
    }
    // Xử lý khi chọn Ngày
    $scope.SelectedDay = new Date;
    $scope.GetDataDay = function () {
        key = $scope.SelectedDay;
        $scope.Day = key;
        GetDay();
    }
    // Xử lý khi click Tab Ngày
    $scope.GetDayClick = function () {
        $scope.Day = $("#day").val();
        GetDay();
    }
    // Xử lý khi click Tab Tháng
    $scope.GetMonthClick = function () {
        $scope.SelectedMonth=$("#month").val();
        $scope.SelectedYear = $("#year").val();
        GetMonth();
    }
    // Init
    $scope.GetMonthInit = function () {       
        GetMonth();
    }
    GetDay();
    //-----
    function DemSoNguoiMuonSach() {
        $http({
            method: "post",
            url: "/ThongKe/MuonSachJson_ssdm",
        }).then(function (response) {
            if (response != null) {
                $scope.SoSachDuocMuon = response.data[0];
                $scope.SoNguoiMuonSach = response.data[1];
            }
        })
    }
})
//======================================END Thống Kê===================================

