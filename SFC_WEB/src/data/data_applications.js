var dataApplications = [
    {
        Name: 'Ambit',
        Route: "/Home/Ambit",
        Image: "assets/img/ambit.png",
        Style: "background-color: #004675; border: 3px solid #0d5c91; box-shadow: 2px 2px 2px #919191; "
    },
    {
        Name: 'QM',
        Route: "/Home/QM",
        Image: "assets/img/QM.png",
        Style: "background-color: #FDBF00; border: 3px solid #eaaf00; box-shadow: 2px 2px 2px #919191; "        
    },
    {
        Name: 'TCQS',
        Route: "/Home/TCQS",
        Image: "assets/img/TCQS.png",
        Style: "background-color: #31BAFD; border: 3px solid #2B9ED8; box-shadow: 2px 2px 2px #919191; "        
    },  
    {
        Name: 'Query 6',
        Route: "/Home/Query6",
        Image: "assets/img/query.png",
        Style: "background-color: #4ABBA4; border: 3px solid rgb(255 255 255);; box-shadow: 2px 2px 2px #919191; "   
    }, 
    {
        Name: 'Query INV2',
        Route: "/Home/QueryINV2",
        Image: "assets/img/Query6.png",
        Style: "background-color: #4ABBA4; border: 3px solid #43A994; box-shadow: 2px 2px 2px #919191; "   
    },   
    {
        Name: 'QueryRepair',
        Route: "/Home/QueryRepair",
        Image: "assets/img/repairQuery.png",
        Style: "background-color: rgb(93 187 74); border: 3px solid rgb(250 250 250); box-shadow: 2px 2px 2px #919191; "   
    }, 
    {
        Name: 'BONEPILE',
        Route: "/Home/BonePile",
        Image: "assets/img/Ponepile.png",
        Style: "background-color: #DA552F; border: 3px solid #bc3610; box-shadow: 2px 2px 2px #919191; "   
    },
    {
        Name: 'MODEL DESC T2',
        Route: "/Home/ModelDestT2",
        Image: "assets/img/repair.png",
        Style: "background-color: #473080; border: 3px solid #584d8e; box-shadow: 2px 2px 2px #919191; "  
    },
    {
        Name: 'Repair Search',
        Route: "/Home/RepairSearch",
        Image: "assets/img/repair.png",
        Style: "background-color: #473080; border: 3px solid #584d8e; box-shadow: 2px 2px 2px #919191; "  
    },
    {
        Name: 'Lock Station',
        Route: "/Home/LockStation",
        Image: "assets/img/lock.png",
        Style: "background-color: #254B70; border: 3px solid #584d8e; box-shadow: 2px 2px 2px #919191; "
    },
    {
        Name: 'Lock History',
        Route: "/Home/LockHistory",
        Image: "assets/img/history.png",
        Style: "background-color: #E05B49; border: 3px solid #C95242; box-shadow: 2px 2px 2px #919191; "
    },
    {
        Name: 'Configs',
        Route: "/Home/ConfigApp",
        Image: "assets/img/config.png",
        Style: "background-color: #E05B49; border: 3px solid #C95242; box-shadow: 2px 2px 2px #919191; "
    },
    {
        Name: 'Query Model',
        Route: "/Home/QueryModel",
        Image: "assets/img/Query6.png",
        Style: "background-color: #4ABBA4; border: 3px solid #43A994; box-shadow: 2px 2px 2px #919191; " 
    },    
    {
        Name: 'TrapTest',
        Route: "/Home/TrapTest",
        Image: "assets/img/TCQS.png",
        Style: "background-color: #E05B49; border: 3px solid #C95242; box-shadow: 2px 2px 2px #919191; "
    },
    {
        Name: 'SHIPTOFILE',
        Route: "/Home/SHIPTOFILE",
        Image: "assets/img/shiptofile.png",
        Style: "background-color: #E05B49; border: 3px solid #C95242; box-shadow: 2px 2px 2px #919191; border-radius: 8px;"
    },
    {
        Name: 'SAGEMCOM',
        Route: "/Home/SAGEMCOM",
        Image: "assets/img/shiptofileubee.png",
        Style: "background-color: #E05B49; border: 3px solid #C95242; box-shadow: 2px 2px 2px #919191;border-radius: 8px; "
    },
    {
        Name: 'SAGEMCOMFOX',
        Route: "/Home/SAGEMCOMFOX",
        Image: "assets/img/shiptofileubee.png",
        Style: "background-color: #E05B49; border: 3px solid #C95242; box-shadow: 2px 2px 2px #919191;border-radius: 8px; "
    },
    {
        Name: 'BakeNO',
        Route: "/Home/BakeNO",
        Image: "assets/img/ornn.png",
        Style: "background-color: rgb(248 85 37); border: 3px solid rgb(248 85 37); box-shadow: 2px 2px 2px #919191; "   
    },   
    {
        Name: 'PQE_NIC',
        Route: "/Home/PQE_NIC",
        Image: "assets/img/PQE.png",
        Style: "background-color: rgb(217 190 96); border: 3px solid rgb(255 255 255); box-shadow: 2px 2px 2px #919191; "
    }, 
    {
        Name: 'PQE_TELIT',
        Route: "/Home/PQE_TELIT",
        Image: "assets/img/config.png",
        Style: "background-color: #E05B49; border: 3px solid #C95242; box-shadow: 2px 2px 2px #919191; "
    },     
    {
        Name: 'SHIPDATA',
        Route: "/Home/SHIPDATA",
        Image: "/assets/img/shipdata.png",
        Style: "background-color: rgb(0 81 100); border: 3px solid rgb(0 81 100); box-shadow: 2px 2px 2px #919191;border-radius: 8px; "
    },
    {
        Name: 'GERENAL_QUERY',
        Route: "/Home/GERENAL_QUERY",
        Image: "assets/img/Query6.png",
        Style: "background-color: #4ABBA4; border: 3px solid #43A994; box-shadow: 2px 2px 2px #919191; "   
    },
    {
        Name: 'IMPORT EXCEL',
        Route: "/Home/ImportExcel",
        Image: "/assets/img/excelImport.png",
        Style: "background-color: rgb(24 112 65); border: 3px solid rgb(24 112 65); box-shadow: 2px 2px 2px #919191;border-radius: 8px; "
    }
];
export default dataApplications;