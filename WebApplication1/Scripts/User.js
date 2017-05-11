/// <reference path="angular.js" />
/// <reference path="App.js" />

app.controller('personCtrl', function ($scope, $http, userService) {

    $scope.UserData = null;
    personService.getallrecords().then(function (d) {
        $scope.UserData = d.data;
    }, function (response) {
        alert('error occurred' + response.data.ExceptionMesage);
    });

    $scope.User = {
        Id: '',
        FirstName: '',
        LastName: '',
        Weight: '',
        Height: '' 
    };

    $scope.clear = function () {
        $scope.User.Id = '',
        $scope.User.FirstName = '',
        $scope.User.LastName = '',
        $scope.User.Weight = ''
        $scope.addnewdiv = false;
        $scope.updatediv = false;
    };
    //Add new record

    $scope.save = function () {
        if ($scope.User.FirstName != '' && $scope.User.LastName != '' && $scope.User.Age != '' && $scope.User.Gender != '') {
            $http({
                method: 'POST',
                url: 'api/Customer/AddUser',
                data: $scope.User

            }).then(function successCallback(response) {
                $scope.UserData.push(response.data);
                $scope.clear();
                alert('Inserted successfully!!');
                $scope.addnewdiv = false;
            }, function errorCallback(response) {

                alert('error:' + response.data.ExceptionMesage);
            });
        }
        else {
            alert('Please enter all the values!!');
        }

    };

    //Edit records
    $scope.edit = function (data) {
        $scope.User = { Id: data.Id, FirstName: data.FirstName, LastName: data.LastName, Age: data.Age, Gender: data.Gender, City: data.City }
        $scope.updatediv = true;
    };

    //Cancel record

    $scope.cancel = function () {
        $scope.clear();
    };

    //Update record
    $scope.update = function () {
        if ($scope.User.FirstName != '' && $scope.User.LastName != '' && $scope.User.Age != '' && $scope.User.Gender != '') {
            $http({
                method: 'PUT',
                url: 'api/User/UpdateUser/' + $scope.User.Id,
                data: $scope.User

            }).then(function successCallback(response) {
                $scope.userData = response.data;
                $scope.clear();
                alert('Updated successfully!!');
                $scope.updatediv = false;
            }, function errorCallback(response) {

                alert('error:' + response.data.ExceptionMesage);
            });
        }
        else {
            alert('Please enter all the values!!');
        }
    };

    //Delete record
    $scope.delete = function (index) {
        $http({
            method: 'DELETE',
            url: 'api/User/DeleteUser/' + $scope.UserData[index].Id,

        }).then(function successCallback(response) {
            $scope.UserData.splice(index, 1);
            alert('Record deleted successfully');
        }, function failureCallback(response) {
            alert('error:' + response.data.ExceptionMesage)
        });

    };
});