(function () {
    var app = angular.module('notifyApp', []);


    var Notifyctrl = function ($scope,$http) {

     
        $scope.newemail = [];
        $scope.emails = {};
       $scope.savedmails = [];
     //   $scope.mails = [];

        $scope.addemail = function () {
           // alert($scope.emails.address);
            $scope.newemail.push({ address: $scope.emails.address });
            $scope.emails = "";

        };



      //  var self = this;
    
        $scope.data = null;
        $scope.selectedItem = null;
        $scope.searchText = null;
    
        $scope.querySearch = function (query) {
            $http.get("/Notify/GetLicensedata", {search: escape(query)} )
              .then(function(result) {
                  $scope.data = result.data.lic_nos;
                  return result.data.lic_nos;
              })
            };
          //  alert($scope.emails.address);
        var fun = function () {

            $http.get("/Notify/Emails").then(function (response) {
                //    alert(response);
                // angular.copy(response.emails, $scope.savedmails);

                //   console.log($scope.savedmails);
                //  $scope.mails = response.emails;
                $scope.mails = response.data.emails;
               // console.log($scope.mails);
                // alert($scope.mails);



            }, function () {
                alert("fail");
            }
            ).finally(function () { $scope.isBusy = false; });
        };
        fun();
            //$scope.newemail.push({ address: $scope.emails.address });

            $scope.deleteServeremail =
                function ($index, Emailid) {
                    $scope.mails.splice($index, 1);
                    $scope.isBusy = true;
                  //  alert(Emailid);
                    
                    $http.post("/Notify/DeleteEmail", {Emailid: Emailid}).then(function (response) {
                        alert("success");
                        //fun();
                    }, function () {
                        alert("fail");
                    }
                    ).finally(function () { $scope.isBusy = false; });
                   

                    //$scope.$emit('emailDeleted', email);

                };
        

        $scope.deleteemail = 
            function ($index, email) {
                $scope.newemail.splice($index, 1);
                //$scope.$emit('emailDeleted', email);

        };
        $scope.savedata = function () {
            $scope.isBusy = true;
            alert($scope.emails.address);
            $http.post("/Notify/Save", $scope.newemail).then(function (response)
            {
                alert("success");
                fun();
               $scope.newemail=[];
                $scope.emails = "";
            }, function ()
            {
                alert("fail");
            }
            ).finally(function () { $scope.isBusy = false; });

            //$scope.newemail.push({ address: $scope.emails.address });

        }; 


    };

   // function Notifyctrl() { };


    app.controller("Notifyctrl", Notifyctrl);
}());