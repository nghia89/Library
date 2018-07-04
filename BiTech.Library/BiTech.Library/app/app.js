
var app = angular.module('MyApp', ['chart.js']);

//(function (app) {
//    app.factory('apiService', apiService);

//    apiService.$inject = ['$http', 'notificationService', 'authenticationService'];

//    function apiService($http, notificationService, authenticationService) {
//        return {
//            get: get,

//        }
//        function get(url, params, success, failure) {
//            authenticationService.setHeader();
//            $http.get(url, params).then(function (result) {
//                success(result);
//            }, function (error) {
//                failure(error);
//            });
//        }
//    }
//});

app.controller('Statistic', function ($scope, $http, $location) {
    Chart.defaults.global.colours = [
    { // blue
        fillColor: "rgba(151,187,205,0.2)",
        strokeColor: "rgba(151,187,205,1)",
        pointColor: "rgba(151,187,205,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(151,187,205,0.8)"
    },
    //{ // light grey
    //    fillColor: "rgba(220,220,220,0.2)",
    //    strokeColor: "rgba(220,220,220,1)",
    //    pointColor: "rgba(220,220,220,1)",
    //    pointStrokeColor: "#fff",
    //    pointHighlightFill: "#fff",
    //    pointHighlightStroke: "rgba(220,220,220,0.8)"
    //},
    { // red
        fillColor: "rgba(247,70,74,0.2)",
        strokeColor: "rgba(247,70,74,1)",
        pointColor: "rgba(247,70,74,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(247,70,74,0.8)"
    },
    //{ // green
    //    fillColor: "rgba(70,191,189,0.2)",
    //    strokeColor: "rgba(70,191,189,1)",
    //    pointColor: "rgba(70,191,189,1)",
    //    pointStrokeColor: "#fff",
    //    pointHighlightFill: "#fff",
    //    pointHighlightStroke: "rgba(70,191,189,0.8)"
    //},
    { // yellow
        fillColor: "rgba(253,180,92,0.2)",
        strokeColor: "rgba(253,180,92,1)",
        pointColor: "rgba(253,180,92,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(253,180,92,0.8)"
    },
    { // grey
        fillColor: "rgba(148,159,177,0.2)",
        strokeColor: "rgba(148,159,177,1)",
        pointColor: "rgba(148,159,177,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(148,159,177,0.8)"
    },
    { // dark grey
        fillColor: "rgba(77,83,96,0.2)",
        strokeColor: "rgba(77,83,96,1)",
        pointColor: "rgba(77,83,96,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(77,83,96,1)"
    }
    ];



     




    //apiService.$inject = ['$http', 'notificationService', 'authenticationService'];
    $scope.data = {
        model:null,
        items:[{ code: 1, name: 'Tháng 01' }, { code: 2, name: 'Tháng 02' },
            { code: 3, name: 'Tháng 03' }, { code: 4, name: 'Tháng 04' },
            { code: 5, name: 'Tháng 05' }, { code: 6, name: 'Tháng 06' },
            { code: 7, name: 'Tháng 07' }, { code: 8, name: 'Tháng 08' },
            { code: 9, name: 'Tháng 09' }, { code: 10, name: 'Tháng 10' },
            { code: 11, name: 'Tháng 11' }, { code: 12, name: 'Tháng 12' },
        ]
    };
    $scope.selectedUser = $scope.data.items[0];
    $scope.day =$scope.data.model;
    $scope.changedValue = function (item) {
        $scope.itemList.push(item.name);
    }

  


    $scope.tabledata = [];
    $scope.labels = ['Tháng 01', 'Tháng 02', 'Tháng 03', 'Tháng 04', 'Tháng 05', ' Tháng 06', 'Tháng 07', 'Tháng 08', 'Tháng 09', ' Tháng 10', 'Tháng 11', 'Tháng 12'];
    //$scope.labels = ['Tháng 01'];

    $scope.series = ['phiếu Mượn Trong Năm', 'Số Người Mượn Trong Năm', 'Số Người không Trả'];
    $scope.chartdata = [];

    $scope.switchLanguage = function () {
        langKey = $scope.selected;

        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    year: langKey
                }
            }

            $http({
                method: "get",
                url: "http://localhost:64002/Statistic/BieuDoPhieuMuon?&month=" + config.param.day + "&year=" + config.param.year,
            }).then(function (response) {
                if (response.data) {
                    $scope.tabledata = response.data;
                    var labels = [];
                    var chartData = [];

                    var lsoPhieuMuonTrongNam = [];
                    var lsoNguoiMuonSachTrongNam = [];
                    var lsoNguoiKhongTraTrongNam = [];
                    //$.each(response.data, function (i, item) {
                    //    labels.push('1');
                    //    revenues.push(item.lsoPhieuMuonTrongNam);
                    //    benefits.push(item.lsoNguoiMuonSachTrongNam);
                    //    benefits.push(item.lsoNguoiKhongTraTrongNam);
                    //});

                    //response.data.lsoNguoiMuonSachTrongNam.forEach(function (i, item) {
                    //    benefits.push(item);
                    //});
                    response.data.lsoPhieuMuonTrongNam.forEach(function (i, index) {
                        lsoPhieuMuonTrongNam.push(i);
                    });
                    response.data.lsoNguoiMuonSachTrongNam.forEach(function (i, index) {
                        lsoNguoiMuonSachTrongNam.push(i);
                    });
                    response.data.lsoNguoiKhongTraTrongNam.forEach(function (i, index) {
                        lsoNguoiKhongTraTrongNam.push(i);
                    });
                    //response.data.lsoPhieuMuonTrongNam.forEach(function (i, index) {
                    //    revenues.push(i);
                    //});
                    //response.data.lsoPhieuMuonTrongNam.forEach(function (i, index) {
                    //    revenues.push(i);
                    //});

                    chartData.push(lsoPhieuMuonTrongNam);
                    chartData.push(lsoNguoiMuonSachTrongNam);
                    chartData.push(lsoNguoiKhongTraTrongNam);
                    //chartData.push(benefits);
                    //chartData.push(benefits);

                    $scope.chartdata = chartData;
                    //$scope.labels = labels;
                }
                else {
                    alert("Mã sách không phù hợp");
                }

            })
        }

        getStatistic();
    }
 
});
