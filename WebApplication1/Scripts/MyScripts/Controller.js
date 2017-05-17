app.controller('crudController', function ($scope, crudService) {

    loadRecords(); 
 
    //Function to load all Employee records
    function loadRecords() {
        var promiseGet = crudService.getCustomer(); //The MEthod Call from service
 
        promiseGet.then(function (pl) { $scope.customers = pl.data },
              function (errorPl) {
                  $log.error('failure loading Employee', errorPl);
              });
    }
});