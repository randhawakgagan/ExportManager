(function () {
    var app = angular.module('notifyApp', []);


    var Notifyctrl = function ($scope,$http) {

        $scope.isemail = false;
        $scope.isdata = false;
        $scope.newemail = [];
        $scope.emails = {};
        $scope.savedmails = [];
        $scope.lic_no = "";
        $scope.addemails = [];
        //  $scope.mails = [];
     

        $scope.addemail = function () {
         //  alert($scope.emails.address);
            $scope.newemail.push({ address: $scope.emails.address });
       //     $scope.items = $scope.newemail
       //.map(function (item) {
       //    return item.address;
       //})
       //.join('\n');
            //     $scope.row = $scope.items.length;
            $scope.isemail = true;
           
            $scope.addemails.push($scope.emails.address);
            $scope.emails = "";

        };



      //  var self = this;
    
        //$scope.data = null;
        //$scope.selectedItem = null;
        //$scope.searchText = null;
    
        //$scope.querySearch = function (query) {

        //    $http.get("/Notify/GetLicensedata", {search: escape(query)} )
        //      .then(function(result) {
        //          $scope.datalists = result.data.lic_nos;
        //        //  return result.data.lic_nos;
        //      })
        //};
        //querySearch();
        //  alert($scope.emails.address);
      
        $scope.fun = function () {
        //    alert($scope.lic_no);
            $scope.isemail = false;
            $scope.isdata = false;

            $http.get("/Notify/Emails",  {params: { lic_id: $scope.lic_no }}).then(function (response) {
                //    alert(response);
                // angular.copy(response.emails, $scope.savedmails);

                //   console.log($scope.savedmails);
                //  $scope.mails = response.emails;
                $scope.mails = response.data.emails;
               // console.log($scope.mails);
                // alert($scope.mails);
                $scope.isdata = true;


            }, function () {
                alert("fail");
            }
            ).finally(function () { $scope.isBusy = false; });
        };
       // fun();
            //$scope.newemail.push({ address: $scope.emails.address });

            $scope.deleteServeremail =
                function ($index, Emailid) {
                    $scope.mails.splice($index, 1);
                    $scope.isBusy = true;
                  //  alert(Emailid);
                    
                    $http.post("/Notify/DeleteEmail", {email: Emailid,lic_id:$scope.lic_no}).then(function (response) {
                        alert("success");
                        $scope.fun();
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
            //   alert($scope.emails.address);
            console.log($scope.newemail);

            $http.post("/Notify/Save", { email: $scope.newemail, lic_id: $scope.lic_no }).then(function (response)
            {
                alert("success");
                $scope.fun();
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