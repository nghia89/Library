angular.module("app", ["chart.js"])

  .config(['ChartJsProvider', function (ChartJsProvider) {
      // Configure all charts
      ChartJsProvider.setOptions({
          chartColors: ['#FF5252', '#FF8A80', '#80b6ff', '#c980ff'],
          responsive: true
      });
  }])

  .controller("RadarCtrl", function ($scope) {

      $scope.labels = ["Eating", "Drinking", "Sleeping", "Designing", "Coding", "Cycling", "Running"];
      $scope.options = { legend: { display: true } };
      $scope.series = ["Series A", "Series B", "Series C", "Series D"];

      $scope.data = [
        [65, 59, 90, 81, 56, 55, 40],
        [28, 48, 40, 19, 96, 27, 100],
        [65, 96, 37, 28, 56, 30, 27],
        [48, 28, 30, 59, 26, 37, 60]
      ];

      /*  $scope.datasets: [
              {
                  label: "My First dataset",
                  backgroundColor: "rgba(179,181,198,0.2)",
                  borderColor: "rgba(179,181,198,1)",
                  pointBackgroundColor: "rgba(179,181,198,1)",
                  pointBorderColor: "#fff",
                  pointHoverBackgroundColor: "#fff",
                  pointHoverBorderColor: "rgba(179,181,198,1)",
                  data: [65, 59, 90, 81, 56, 55, 40]
              },
              {
                  label: "My Second dataset",
                  backgroundColor: "rgba(255,99,132,0.2)",
                  borderColor: "rgba(255,99,132,1)",
                  pointBackgroundColor: "rgba(255,99,132,1)",
                  pointBorderColor: "#fff",
                  pointHoverBackgroundColor: "#fff",
                  pointHoverBorderColor: "rgba(255,99,132,1)",
                  data: [28, 48, 40, 19, 96, 27, 100]
              }
          ]*/

  });