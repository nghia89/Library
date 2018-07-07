
var app = angular.module('MyApp', ['chart.js']);

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
    { // light grey
        fillColor: "rgba(220,220,220,0.2)",
        strokeColor: "rgba(220,220,220,1)",
        pointColor: "rgba(220,220,220,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(220,220,220,0.8)"
    },
    { // red
        fillColor: "rgba(247,70,74,0.2)",
        strokeColor: "rgba(247,70,74,1)",
        pointColor: "rgba(247,70,74,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(247,70,74,0.8)"
    },
    { // green
        fillColor: "rgba(70,191,189,0.2)",
        strokeColor: "rgba(70,191,189,1)",
        pointColor: "rgba(70,191,189,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(70,191,189,0.8)"
    },
    { // yellow
        fillColor: "rgba(253,180,92,0.2)",
        strokeColor: "rgba(253,180,92,1)",
        pointColor: "rgba(253,180,92,1)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(253,180,92,0.8)"
    },
    //{ // grey
    //    fillColor: "rgba(148,159,177,0.2)",
    //    strokeColor: "rgba(148,159,177,1)",
    //    pointColor: "rgba(148,159,177,1)",
    //    pointStrokeColor: "#fff",
    //    pointHighlightFill: "#fff",
    //    pointHighlightStroke: "rgba(148,159,177,0.8)"
    //},
    //{ // dark grey
    //    fillColor: "rgba(77,83,96,0.2)",
    //    strokeColor: "rgba(77,83,96,1)",
    //    pointColor: "rgba(77,83,96,1)",
    //    pointStrokeColor: "#fff",
    //    pointHighlightFill: "#fff",
    //    pointHighlightStroke: "rgba(77,83,96,1)"
    //}
    ];



    //apiService.$inject = ['$http', 'notificationService', 'authenticationService'];
    //$scope.data = {
    //    model:null,
    //    items:[{ code: 1, name: 'Tháng 01' }, { code: 2, name: 'Tháng 02' },
    //        { code: 3, name: 'Tháng 03' }, { code: 4, name: 'Tháng 04' },
    //        { code: 5, name: 'Tháng 05' }, { code: 6, name: 'Tháng 06' },
    //        { code: 7, name: 'Tháng 07' }, { code: 8, name: 'Tháng 08' },
    //        { code: 9, name: 'Tháng 09' }, { code: 10, name: 'Tháng 10' },
    //        { code: 11, name: 'Tháng 11' }, { code: 12, name: 'Tháng 12' },
    //    ]
    //};
    //$scope.selectedUser = $scope.data.items[0];
    //$scope.day =$scope.data.model;

    //$scope.changedValue = function (item) {
    //    $scope.itemList.push(item.name);
    //}

  



    $scope.labels = ['Tháng 01', 'Tháng 02', 'Tháng 03', 'Tháng 04', 'Tháng 05', ' Tháng 06', 'Tháng 07', 'Tháng 08', 'Tháng 09', ' Tháng 10', 'Tháng 11', 'Tháng 12'];
    $scope.series = ['phiếu Mượn Trong Năm', 'Số Người Mượn Trong Năm', 'Số Người không Trả sách','Số Sách Được Mượn','Số Người Trả Trể'];
    $scope.chartdataYear = [];
    $scope.loading = true;
    $scope.year = function () {
        langKey = $scope.selected;

        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    month: '',
                    year: langKey
                }
            }

            $http({
                method: "get",
                url: "/Statistic/BieuDoPhieuMuon?&month=" + config.param.month + "&year=" + config.param.year,
            }).then(function (response) {
                if (response.data) {

                    var chartData = [];

                    var lsoPhieuMuonTrongNam = [];
                    var lsoNguoiMuonSachTrongNam = [];
                    var lsoNguoiKhongTraTrongNam = [];
                    var lsoSachDuocMuonTrongNam = [];
                    var lsoNguoiTraTreTrongNam = [];
         
                    response.data.lsoPhieuMuonTrongNam.forEach(function (i, index) {
                        lsoPhieuMuonTrongNam.push(i);
                    });
                    response.data.lsoNguoiMuonSachTrongNam.forEach(function (i, index) {
                        lsoNguoiMuonSachTrongNam.push(i);
                    });
                    response.data.lsoNguoiKhongTraTrongNam.forEach(function (i, index) {
                        lsoNguoiKhongTraTrongNam.push(i);
                    });
                    response.data.lsoSachDuocMuonTrongNam.forEach(function (i, index) {
                        lsoSachDuocMuonTrongNam.push(i);
                    });
                    response.data.lsoNguoiTraTreTrongNam.forEach(function (i, index) {
                        lsoNguoiTraTreTrongNam.push(i);
                    });
                  

                    chartData.push(lsoPhieuMuonTrongNam);
                    chartData.push(lsoNguoiMuonSachTrongNam);
                    chartData.push(lsoNguoiKhongTraTrongNam);
                    chartData.push(lsoSachDuocMuonTrongNam);
                    chartData.push(lsoNguoiTraTreTrongNam);
        
                    //chartData.push(benefits);
                    //chartData.push(benefits);

                    $scope.chartdataYear = chartData;
                    //$scope.labels = labels;
                }
                else {
                    alert("không thể tải dữ liệu");
                }

            })
        }

        getStatistic();
        $scope.loading = false;
    }
 
});



app.controller('MonthCtroller', function ($scope, $http, $location) {
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
   { // green
       fillColor: "rgba(70,191,189,0.2)",
       strokeColor: "rgba(70,191,189,1)",
       pointColor: "rgba(70,191,189,1)",
       pointStrokeColor: "#fff",
       pointHighlightFill: "#fff",
       pointHighlightStroke: "rgba(70,191,189,0.8)"
   },
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
   //{ // dark grey
   //    fillColor: "rgba(77,83,96,0.2)",
   //    strokeColor: "rgba(77,83,96,1)",
   //    pointColor: "rgba(77,83,96,1)",
   //    pointStrokeColor: "#fff",
   //    pointHighlightFill: "#fff",
   //    pointHighlightStroke: "rgba(77,83,96,1)"
   //}
    ];



    //apiService.$inject = ['$http', 'notificationService', 'authenticationService'];
    //$scope.data = {
    //    model:null,
    //    items:[{ code: 1, name: 'Tháng 01' }, { code: 2, name: 'Tháng 02' },
    //        { code: 3, name: 'Tháng 03' }, { code: 4, name: 'Tháng 04' },
    //        { code: 5, name: 'Tháng 05' }, { code: 6, name: 'Tháng 06' },
    //        { code: 7, name: 'Tháng 07' }, { code: 8, name: 'Tháng 08' },
    //        { code: 9, name: 'Tháng 09' }, { code: 10, name: 'Tháng 10' },
    //        { code: 11, name: 'Tháng 11' }, { code: 12, name: 'Tháng 12' },
    //    ]
    //};
    //$scope.selectedUser = $scope.data.items[0];
    //$scope.day =$scope.data.model;

    //$scope.changedValue = function (item) {
    //    $scope.itemList.push(item.name);
    //}




    $scope.labels = ['Ngày 01', 'Ngày 02', 'Ngày 03', 'Ngày 04', 'Ngày 05', ' Ngày 06', 'Ngày 07', 'Ngày 08', 'Ngày 09', ' Ngày 10', 'Ngày 11', 'Ngày 12',
    'Ngày 03', 'Ngày 14', 'Ngày 14', 'Ngày 16', 'Ngày 17', ' Ngày 18', 'Ngày 19', 'Ngày 20', 'Ngày 21', ' Ngày 22', 'Ngày 23', 'Ngày 24',
    'Ngày 25', 'Ngày 26', 'Ngày 27', 'Ngày 28', 'Ngày 29', ' Ngày 30', 'Ngày 31'];
    $scope.series = ['phiếu Mượn Trong Ngày', 'Số Người Mượn Trong Ngày', 'Số Người không Trả sách', 'Số Sách Được Mượn', 'Số Người Trả Trể'];
    $scope.chartdataMonth = [];

    $scope.loading = true;
    $scope.Month = function () {
        langKey = $scope.selected;   
        function getStatisticMonth() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    month: langKey,
                    year: ''

                }
            }

            $http({
                method: "get",
                url: "/Statistic/BieuDoPhieuMuon?&month=" + config.param.month + "&year=" + config.param.year,
            }).then(function (response) {
                if (response.data) {

                    var chartData1 = [];

                    var lsoPMTrongNgay = [];
                    var lsoNguoiMuonTrongNgay = [];
                    var lsoSachDuocMuonTrongNgay = [];
                    var lsoNguoiKhongTraTrongNgay = [];
                    var lsoNguoiTraTreTrongNgay = [];

                    response.data.lsoPMTrongNgay.forEach(function (i, index) {
                        lsoPMTrongNgay.push(i);
                    });
                    response.data.lsoNguoiMuonTrongNgay.forEach(function (i, index) {
                        lsoNguoiMuonTrongNgay.push(i);
                    });
                    response.data.lsoNguoiKhongTraTrongNgay.forEach(function (i, index) {
                        lsoNguoiKhongTraTrongNgay.push(i);
                    });
                    response.data.lsoSachDuocMuonTrongNgay.forEach(function (i, index) {
                        lsoSachDuocMuonTrongNgay.push(i);
                    });
                    response.data.lsoNguoiTraTreTrongNgay.forEach(function (i, index) {
                        lsoNguoiTraTreTrongNgay.push(i);
                    });


                    chartData1.push(lsoPMTrongNgay);
                    chartData1.push(lsoNguoiMuonTrongNgay);
                    chartData1.push(lsoSachDuocMuonTrongNgay);
                    chartData1.push(lsoNguoiKhongTraTrongNgay);
                    chartData1.push(lsoNguoiTraTreTrongNgay);

                    //chartData.push(benefits);
                    //chartData.push(benefits);

                    $scope.chartdataMonth = chartData1;
                    //$scope.labels = labels;
                }
                else {
                    alert("không thể tải dữ liệu");
                }

            })
        }

        getStatisticMonth();
        $scope.loading = false;
    }

});

