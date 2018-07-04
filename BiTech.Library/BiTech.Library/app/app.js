
var app = angular.module('MyApp', ['chart.js']);


app.controller('Statistic', function ($scope, $http) {
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
    $scope.labels = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
    $scope.series = ['Series A', 'Series B'];
    $scope.data = [
      [65, 59, 80, 81, 56, 55, 40],
      [28, 48, 40, 19, 86, 27, 90],
      [28, 48, 40, 19, 86, 27, 20],
      [28, 48, 70, 89, 86, 87, 90],
      [28, 48, 90, 5, 86, 27, 20]
    ];
    $scope.GetAllData = function () {
        $http({
            method: "get",
            url: "",
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
