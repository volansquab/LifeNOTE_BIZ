
    //add this js script into the web page,
    //you want reload once after first load
    window.onload = function () {
        var tab2 = document.getElementById("@IndexModel.activetabname");
        var tab3 = document.getElementById("@IndexModel.activetabname2");
        //considering there aren't any hashes in the urls already
        if (!window.location.hash) {
            //setting window location
            window.location = window.location + '#loaded';
            //using reload() method to reload web page
            setTimeout(function () { tab3.checked = true; }, 500);
            window.location.reload();
            setTimeout(function () { tab2.checked = true; }, 500);
            window.location.reload();
        }
        else
        {
            var tab = document.getElementById("@IndexModel.activetabname");
            tab.checked = true;
        }
    }

    /*alert(document.getElementById("textBox").innerHTML);*/
    var oDoc, sDefTxt, locindx;
    //�^�u��1�̏ꍇ�̓A�N�e�B�u��
        //var tab = document.getElementById("@IndexModel.activetabname");

        var titlev = "";
        var rtime;
        var timeout = false;
        var delta = 2000;
        var chkstr = "";
        /*document.location.reload()*/
        const loader = document.getElementById('loader');
        loader.style = "display:none;";
        loader.disabled;
        var select = document.getElementById("ddlProducts");
        //�������X�g�̈ʒu
        select.options["@IndexModel.locstr"].selected = true;
        //�������X�g�̈ʒu�̓Y�t�t�@�C�� attachfile
        var initattach = "@IndexModel.attachfile"
        if (initattach.length > 0) {
            document.getElementById("upload").innerHTML = "";
            attach.split(',').forEach(function (value) {
                addOption(value);
            })

        }
        else {
            document.getElementById("upload").innerHTML = "";
            addNone();
        }
        //�������X�g�̖��O title
        //�^�u��1�̏ꍇ�̓A�N�e�B�u��
        //tab.checked = true;
        /*tab.draggable = true;*/

    setTimeout(function () { chkstr = document.getElementById("textBox").innerHTML; }, 0);
    //�����ۑ�
    var updtef = true
    const ev = function () {
        var chkstr2 = document.getElementById("textBox").innerHTML
        /*if (updatef != "") {*/
        //alert(chkstr);
        //alert(chkstr2);
        if (chkstr != chkstr2) {
            //�����ۑ�����
            //alert(chkstr);
            //alert(chkstr2);
            textbody = document.getElementById('textBox').innerText;
            htmlbody = document.getElementById('textBox').innerHTML;
            document.getElementById('loader').style = "display: normal;";
            /*alert("start");*/
            const form = document.forms[3];
            FD = new FormData(form);
            // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
            /*FD.append("indx", 'pass');*/
            FD.append("evnt", "save");
            FD.append("textbody", textbody);
            FD.append("htmlbody", htmlbody);
            // aysnchronous fetch ajax
            fetch(form.action,
                {
                    method: form.method,
                    body: FD
                }
            )
                // if any exceptions - log them
                .catch(err => alert("network error: " + err))
                .then(response => {

                    // read json from the response stream
                    // and display the data
                    response.json().then(data => {

                        rtime = new Date();
                        if (timeout === false) {
                            timeout = true;
                            setTimeout(resizeend, delta);
                        }
                    });
                })

            /*}*/
            chkstr = document.getElementById("textBox").innerHTML
        }
    };
    setInterval(ev, 1000);


    function resizeend() {
        if (new Date() - rtime < delta) {
            setTimeout(resizeend, delta);
        } else {
            timeout = false;
            /*alert("HIT");*/
            document.getElementById('loader').style = "display: none;";
        }
    }


    function initDoc() {

       oDoc = document.getElementById("textBox");
       oDoc.scrollTop = 100;
       /*document.location.reload();*/
       const form = document.forms[0];
       FD = new FormData(form);
       // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�

       FD.append("evnt", "openattach");
       // aysnchronous fetch ajax
       fetch(form.action,
           {
               method: form.method,
               body: FD
           }
       )
           // if any exceptions - log them
           .catch(err => console.log("network error: " + err))
           .then((response) => {
               return response.json()�@//������Body����JSON��Ԃ�
           })
           .then((result) => {
               Example3(result);  //�擾����JSON�f�[�^���֐��ɓn��
           })

       sDefTxt = oDoc.innerHTML;


       if (document.compForm.switchMode.checked) { setDocMode(true); }
    }

    function formatDoc(sCmd, sValue) {
        if (validateMode()) { document.execCommand(sCmd, false, sValue); oDoc.focus(); }
    }

    function validateMode() {
        if (!document.compForm.switchMode.checked) { return true; }
        alert("Uncheck \"Show HTML\".");
        oDoc.focus();
        return false;
    }

    function setDocMode(bToSource) {
        var oContent;
        if (bToSource) {
            oContent = document.createTextNode(oDoc.innerHTML);
            oDoc.innerHTML = "";
            var oPre = document.createElement("pre");
            oDoc.contentEditable = false;
            oPre.id = "sourceText";
            oPre.contentEditable = true;
            oPre.appendChild(oContent);
            oDoc.appendChild(oPre);
        } else {
            if (document.all) {
                oDoc.innerHTML = oDoc.innerText;
            } else {
                oContent = document.createRange();
                oContent.selectNodeContents(oDoc.firstChild);
                oDoc.innerHTML = oContent.toString();
            }
            oDoc.contentEditable = true;
        }
        oDoc.focus();
    }

    function printDoc() {
        if (!validateMode()) { return; }
        var oPrntWin = window.open("", "_blank", "width=450,height=470,left=400,top=100,menubar=yes,toolbar=no,location=no,scrollbars=yes");
        oPrntWin.document.open();
        oPrntWin.document.write("<!doctype html><html><head><title>Print<\/title><\/head><body onload=\"print();\">" + oDoc.innerHTML + "<\/body><\/html>");
        oPrntWin.document.close();
    }
    function saveDoc(textbody, htmlbody) {

        const form = document.forms[3];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("textbody", textbody);
        FD.append("htmlbody", htmlbody);
        FD.append("indx", 'pass');
        FD.append("evnt", "save");

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
        // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => { document.location.reload(); });
            })

    }

    function ChangeTytle() {

        const form = document.forms[1];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("indx", 'pass');
        FD.append("evnt", "tytlechange");

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    document.location.reload();
                });
            })
    }



    function Newdoc() {

        const form = document.forms[0];
        const titletext = document.getElementById("title2").value;
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("title", titletext);
        /*alert(titletext);*/
        FD.append("textbody", "");
        FD.append("htmlbody", "");
        FD.append("indx", "pass");
        FD.append("evnt", "partialnew");

         /*aysnchronous fetch ajax*/
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textbox.innerHTML = data
                    document.location.reload();
                });
            })
    }

    function deleteDoc() {
        const form = document.forms[3];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("indx", 'pass');
        FD.append("evnt", "deletedoc");

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    document.location.reload();
                });
            })
        }

        function Up() {
            const form = document.forms[3];
            FD = new FormData(form);
            // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
            FD.append("indx", "pass");
            FD.append("evnt", "up");
            //textBox.innerHTML = "";

            // aysnchronous fetch ajax
            fetch(form.action,
                {
                    method: form.method,
                    body: FD
                }
            )
                // if any exceptions - log them
                .catch(err => console.log("network error: " + err))
                .then(response => {

                    // read json from the response stream
                    // and display the data
                    response.json().then(data => {
                        document.location.reload();
                        //textBox.innerHTML = data
                    })
                .then((result) => {
                    setTimeout(function () { chkstr = document.getElementById("textBox").innerHTML; }, 0);
                    });
                })
        }
        function Down() {
            const form = document.forms[3];
            FD = new FormData(form);
            // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
            FD.append("indx", "pass");
            FD.append("evnt", "down");

            // aysnchronous fetch ajax
            fetch(form.action,
                {
                    method: form.method,
                    body: FD
                }
            )
                // if any exceptions - log them
                .catch(err => console.log("network error: " + err))
                .then(response => {

                    // read json from the response stream
                    // and display the data
                    response.json().then(data => {
                        //textBox.innerHTML = data
                        document.location.reload();
                    })
                .then((result) => {
                    setTimeout(function () { chkstr = document.getElementById("textBox").innerHTML; }, 0);
                    });
                })

    }

    function Left() {
        const form = document.forms[3];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("indx", "pass");
        FD.append("evnt", "left");


        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textBox.innerHTML = data
                    document.location.reload();
                })
                    .then((result) => {
                        setTimeout(function () { chkstr = document.getElementById("textBox").innerHTML; }, 0);
                    });
            })

    }

    function Right() {
        const form = document.forms[3];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("indx", "pass");
        FD.append("evnt", "right");


        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textBox.innerHTML = data
                    document.location.reload();
                })
                    .then((result) => {
                        setTimeout(function () { chkstr = document.getElementById("textBox").innerHTML; }, 0);
                    });
            })

    }
    function TabClick(id) {

        var tabno = id.split('_');
        const form = document.forms[3];
        FD = new FormData(form);
        FD.append("tabno", tabno[1]);
        FD.append("evnt", "tabclick");
        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    document.location.reload();
                });
            })
    }
    function Search() {
        const textbox = document.getElementById("search");
        const elements = document.getElementsByName("field");

        @*element = document.getElementById('global');*@
        const value = textbox.value;
        @*const check = element.checked;*@


        // �I����Ԃ̒l���擾
        for (var a = "", i = elements.length; i--;) {
            if (elements[i].checked) {
                var a = elements[i].value;
                break;
            }
        }
        const form = document.forms[0];
        FD = new FormData(form);
        FD.append("keyword", value);
        FD.append("evnt", "search");
        FD.append("field", a);
        @*FD.append("global", check);*@

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    document.location.reload();
                });
            })


    }

    // only one form - select it!
    const form = document.forms[4];
    // attach onsubmit
    var textstr2 = document.getElementById('textBox');
    textstr2.addEventListener("paste", async function (event) {
        setTimeout(function () {


            //�n�C�p�[�����N�̎����t�^�F�z�[���y�[�W
            //var elms = document.getElementById('textBox').getElementsByTagName('div');
            var pasteText = document.getElementById('textBox').innerText;
            pasteText = pasteText.replace(/\r?\n/g, '\n');
            pasteText = pasteText.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;")
            pasteText = pasteText.replace('�@', ' ');
            var startkey = 'http';
            var stopkey1 = ' ';
            var stopkey2 = '\n';
            var startloc = 0;
            var endloc = 0;
            var startcount = 0;
            var urlarray = [];
            // �ʏ��for���ōs��
            for (var i = 0; i < pasteText.length; i++) {
                if (pasteText.slice(i, i + startkey.length) === startkey) {
                    startloc = i;
                    /*startcount = startcount + 1;*/
                    /*endloc = i + 4;*/
                    var url_flag = false
                    for (var k = startloc; k < pasteText.length; k++) {
                        if (pasteText.slice(k, k + stopkey1.length) === stopkey1) {
                            endloc = k;
                            i = k + stopkey1.length;
                            url_flag = true;
                            break;
                        }
                        else if (pasteText.slice(k, k + stopkey2.length) === stopkey2) {
                            endloc = k;
                            i = k + stopkey1.length;
                            url_flag = true;
                            break;
                        }
                        else {

                        }
                    }
                    if (url_flag === true) {

                        urlarray.push(pasteText.slice(startloc, endloc));


                    }
                }
            }
            var innerText = document.getElementById('textBox').innerHTML;
            {
                //element = element.replace('<spert3>', '&');
                urlarray.forEach(function (element) {
                    if (~innerText.indexOf('<a href="' + element + '">' + element + '</a>')) {

                    }
                    else {
                        /*alert(element);*/
                        innerText = innerText.replace(element, '<a href="' + element + '">' + element + '</a>');
                        document.getElementById('textBox').innerHTML = innerText;

                    }
                });
            }

            //�n�C�p�[�����N�̎����t�^�F�T�[�o�[
            //var elms = document.getElementById('textBox').getElementsByTagName('div');
            var pasteText = document.getElementById('textBox').innerText;
            pasteText = pasteText.replace(/\r?\n/g, '\n');

            pasteText = pasteText.replace('�@', ' ');
            var startkey = '\\\\';
            var stopkey1 = ' ';
            var stopkey2 = '\n';
            var startloc = 0;
            var endloc = 0;
            var urlarray = [];
            // �ʏ��for���ōs��
            for (var i = 0; i < pasteText.length; i++) {
                if (pasteText.slice(i, i + startkey.length) === startkey) {
                    startloc = i;
                    endloc = i + 4;
                    var url_flag = false
                    for (var k = startloc; k < pasteText.length; k++) {
                        if (pasteText.slice(k, k + stopkey1.length) === stopkey1) {
                            endloc = k;
                            i = k + stopkey1.length;
                            url_flag = true;
                            break;
                        }
                        else if (pasteText.slice(k, k + stopkey2.length) === stopkey2) {
                            endloc = k;
                            i = k + stopkey1.length;
                            url_flag = true;
                            break;
                        }
                        else {

                        }
                    }
                    if (url_flag === true) {
                        urlarray.push(pasteText.slice(startloc, endloc));


                    }
                }
            }
            var innerText = document.getElementById('textBox').innerHTML;
            {
                urlarray.forEach(function (element) {
                    if (~innerText.indexOf('<a href="' + 'file:///' + element + '">' + 'file:///' + element + '</a>')) {

                    }
                    else {
                        innerText = innerText.replace(element, '<a href="' + 'file:///' + element + '">' + 'file:///' + element + '</a>');
                        document.getElementById('textBox').innerHTML = innerText;

                    }
                });
            }
            //�n�C�p�[�����N�̎����t�^�F���[�J�������N
            pasteText = document.getElementById('textBox').innerText;
            var startkey = 'C:\\';
            var stopkey1 = ' ';
            var stopkey2 = '\n';
            var startloc = 0;
            var endloc = 0;
            var urlarray2 = [];

            // �ʏ��for���ōs��
            for (var i = 0; i < pasteText.length; i++) {
                if (/[a-zA-Z]:\\/.test(pasteText.slice(i, i + 3)) === true) {
                    startloc = i;
                    endloc = i + 4;
                    var url_flag = false
                    for (var k = startloc; k < pasteText.length; k++) {
                        if (pasteText.slice(k, k + stopkey1.length) === stopkey1) {
                            endloc = k;
                            i = k + stopkey1.length;
                            url_flag = true;
                            break;
                        }
                        else if (pasteText.slice(k, k + stopkey2.length) === stopkey2) {
                            endloc = k;
                            i = k + stopkey1.length;
                            url_flag = true;
                            break;
                        }
                        else {

                        }
                    }
                    if (url_flag === true) {
                        urlarray2.push(pasteText.slice(startloc, endloc));
                    }
                }
            }
            var innerText = document.getElementById('textBox').innerHTML;
            {
                urlarray2.forEach(function (element) {
                    if (~innerText.indexOf('<a href="' + 'file:///' + element + '">' + 'file:///' + element + '</a>')) {

                    }
                    else {
                        innerText = innerText.replace(element, '<a href="' + 'file:///' + element + '">' + 'file:///' + element + '</a>');
                        document.getElementById('textBox').innerHTML = innerText;
                    }
                });
            }


     }, 0);

}, false);

    function GetBody(indxstr) {

            const form = document.forms[2];
            FD = new FormData(form);
            //�X�V����A�N�e�B�u�ɂ���B
            document.getElementById("uploadd").disabled = true;
            document.getElementById("dropdownMenuButton2").disabled = true;
            document.getElementById("up").disabled = true;
            document.getElementById("down").disabled = true;
            document.getElementById("leftmove").disabled = true;
            document.getElementById("rightmove").disabled = true;

            locindx = indxstr;
            // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
            FD.append("indx", indxstr);
            FD.append("evnt", "listclick");


            // aysnchronous fetch ajax
            fetch(form.action,
                {
                    method: form.method,
                    body: FD
                }
            )
                // if any exceptions - log them
                .catch(err => console.log("network error: " + err))
                .then((response) => {
                    return response.json()�@//������Body����JSON��Ԃ�
                })
                .then((result) => {
                    Example(result);  //�擾����JSON�f�[�^���֐��ɓn��
                    setTimeout(function () { chkstr = document.getElementById("textBox").innerHTML; }, 0);
                })

            oDoc = document.getElementById("textBox");
            oDoc.scrollTop = 100;

        }


    window.addEventListener('scroll', function (e) {
        var target = document.getElementById('textBox');
        //target.onscroll = () => alert(target.scrollTop);
    });

        //JSON�f�[�^�������Ɏ󂯎����DOM������s���֐����쐬
        function Example(jsonObj) {
            const parsedObj = JSON.parse(jsonObj);
            textBox.innerHTML = parsedObj.body;
            //alert(textBox.innerHTML);
            title.value = parsedObj.title;
            titlev = parsedObj.title;
            updatedate.value =  parsedObj.uploaddate;
            author.value =  parsedObj.author;
            var attach = parsedObj.attachfile;
            if (attach.length > 0) {
                document.getElementById("upload").innerHTML = "";
                attach.split(',').forEach(function (value) {
                    addOption(value);
                })

            }
            else
            {
                document.getElementById("upload").innerHTML = "";
                addNone();
            }


        }

        //JSON�f�[�^�������Ɏ󂯎����DOM������s���֐����쐬
        function Example2(jsonObj) {
            const parsedObj = JSON.parse(jsonObj);
            textBox.innerHTML = parsedObj.body;

            var link = parsedObj.attachlink;
            link = link.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
            var winGoogle = window.open(link);
    }

    function Example3(jsonObj) {
        const parsedObj = JSON.parse(jsonObj);
        var attach = parsedObj.attachfile;
        if (attach.length > 0) {
            document.getElementById("upload").innerHTML = "";
            attach.split(',').forEach(function (value) {
                addOption(value);
            })

        }
        else {
            document.getElementById("upload").innerHTML = "";
            addNone();
        }


    }
        function addOption(value) {
            // select�^�O���擾����
            var select = document.getElementById("upload");
            // option�^�O���쐬����
            var option = document.createElement("option");
            // option�^�O�̃e�L�X�g��4�ɐݒ肷��
            option.text = value;
            option.index = 1;
            // select�^�O�̎q�v�f��option�^�O��ǉ�����
            select.appendChild(option);
            document.getElementById("uploadd").disabled = false;
            document.getElementById("dropdownMenuButton2").disabled = false;
            document.getElementById("up").disabled = false;
            document.getElementById("down").disabled = false;
            document.getElementById("leftmove").disabled = false;
            document.getElementById("rightmove").disabled = false;
        }

        function addNone() {
            // select�^�O���擾����
            var select = document.getElementById("upload");
            // option�^�O���쐬����
            var option = document.createElement("option");
            // option�^�O�̃e�L�X�g��4�ɐݒ肷��
            option.text = "�t�@�C������";
            // select�^�O�̎q�v�f��option�^�O��ǉ�����
            select.appendChild(option);
            document.getElementById("uploadd").disabled = false;
            document.getElementById("dropdownMenuButton2").disabled = false;
            document.getElementById("up").disabled = false;
            document.getElementById("down").disabled = false;
            document.getElementById("leftmove").disabled = false;
            document.getElementById("rightmove").disabled = false;
        }
    function objectsToArray(data, columns) {
        var dataArray = [];
        for (var i in data) {
            var itemArray = [];
            for (var j in columns) {
                itemArray.push(data[i][columns[j]]);
            }
            dataArray.push(itemArray);
        }
        return dataArray;
    }
    //�C����
        function TempSetting() {
            const select = document.forms[2].ddProducts
            const index = select.selectedIndex;

            const form = document.forms[1];
            FD = new FormData(form);
            // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
            FD.append("evnt", "upload1");
            FD.append("indx", index);
            // aysnchronous fetch ajax
            fetch(form.action,
                {
                        method: form.method,
                        body: FD
                }
            )
            .then(response => document.location.reload())
    }
    function addItems() {
        var numberOfItems = 5;
        for (var i = 0; i < numberOfItems; i++) {
            var ele = document.createElement("a");
            ele.classList = "dropdown-item";
            ele.href = "#";
            ele.innerText = "" + i;
            document.querySelector(".dropdown-menu").appendChild(ele);
        }
    }
    function Attach(value) {
        const select = document.forms[2].ddProducts
        const index = select.selectedIndex;
        if (value != "�t�@�C������") {
            const form = document.forms[2];
            FD = new FormData(form);
            // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
            FD.append("evnt", "attach");
            FD.append("attachname", value);
            FD.append("indx", index);
            // aysnchronous fetch ajax
            fetch(form.action,
                {
                    method: form.method,
                    body: FD
                }
            )
                // if any exceptions - log them
                .catch(err => console.log("network error: " + err))
                .then((response) => {
                    return response.json()�@//������Body����JSON��Ԃ�
                })
                .then((result) => {
                    Example2(result);  //�擾����JSON�f�[�^���֐��ɓn��
                })
        }
    }


    function showGenre1() {
        if (titlev == "") {
            document.getElementById("change").innerText = "�ύX����^�C�g������͂��Ă��������B";
            document.getElementById("title").value = @(Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(IndexModel.title)));
            //
        } else
            document.getElementById("change").innerText = "�ύX����^�C�g������͂��Ă��������B";


    }


    function showGenre2(item) {
        /*newDoc();*/
        if (titlev == "") {
            document.getElementById("newtitle").innerText = "�h�L�������g��ǉ����܂��B";
            document.getElementById("title2").value = @(Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(IndexModel.title)));
            //
        } else
            document.getElementById("newtitle").innerText = "�h�L�������g��ǉ����܂��B";

    }
    function showGenre3(item1,item2) {
        saveDoc(item1, item2);
    }
    function showGenre4() {
        if (titlev == "") {
            document.getElementById("deletetitle").innerText = "�^�C�g���u" + @(Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(IndexModel.title))) + "�v���폜���܂��B";
        } else
            document.getElementById("deletetitle").innerText = "�^�C�g���u" + titlev + "�v���폜���܂��B";

    }

    function TytleChange() {
        var title = document.getElementById("title").value;
        const form = document.forms[0];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("evnt", "tytlechanged");
        //FD.append("indx", "pass");
        FD.append("title", title);
        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textbox.innerHTML = data
                    document.location.reload();
                });
            })
    }

    function TabNameChange() {
        alert("hit");
        var tabname = document.getElementById("tabname2").value;
        const form = document.forms[0];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("evnt", "tabnamechanged");
        //FD.append("indx", "pass");
        FD.append("tabname", tabname);
        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textbox.innerHTML = data
                    document.location.reload();
                });
            })
    }

    function DeleteDoc() {
        const form = document.forms[0];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("evnt", "deleted");

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textbox.innerHTML = data
                    document.location.reload();
                });
            })
    }
    function AddTab() {
        var addtab = document.getElementById("tabname").value;
        const form = document.forms[0];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("evnt", "addtab");
        FD.append("addtab", addtab);

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textbox.innerHTML = data
                    document.location.reload();
                });
            })
    }
    function InactiveTab() {
        var e = document.getElementById("inactive");
        var value = e.value;
        var text = e.options[e.selectedIndex].text;
        const form = document.forms[0];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("evnt", "inactivetab");
        FD.append("inactivetab", text);
        FD.append("deleteno", value);

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textbox.innerHTML = data
                    document.location.reload();
                });
            })
    }
    function ShowTab() {
        var e = document.getElementById("show");
        var value = e.value;
        var text = e.options[e.selectedIndex].text;

        const form = document.forms[0];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("evnt", "showtab");
        FD.append("showtab", text);
        FD.append("deleteno", value);

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textbox.innerHTML = data
                    document.location.reload();
                });
            })
    }
    function DeleteTab() {
        var e = document.getElementById("deletetab2");
        var value = e.value;
        var text = e.options[e.selectedIndex].text;

        const form = document.forms[0];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("evnt", "deletetab");
        FD.append("deletetab", text);
        FD.append("deleteno", value);

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    //textbox.innerHTML = data
                    document.location.reload();
                });
            })
    }
    function Setting() {
        const form = document.forms[3];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("indx", 'pass');
        FD.append("evnt", "setting");

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    document.location.reload();
                });
            })
    }

    function myHandler2(e) {
        if (e.target.tagName.toLowerCase() === 'a' && event.target.id == "") {
            //http�Ŏn�܂�
            if (~event.target.innerHTML.indexOf('http')) {
                window.open(event.target.href, '_blank');
                //����ȊO
            } else {
                alert("���[�J���t�@�C�����J���܂��B");
                window.open(event.target.href, '_blank');
            }
        }
    }

    $(document).ready(myHandler2);
    $("#textBox").on("click", myHandler2);
    function Expand() {
        const form = document.forms[3];
        FD = new FormData(form);
        // �f�[�^�� FormData �I�u�W�F�N�g�ɓ������܂�
        FD.append("indx", 'pass');
        FD.append("evnt", "expand");

        // aysnchronous fetch ajax
        fetch(form.action,
            {
                method: form.method,
                body: FD
            }
        )
            // if any exceptions - log them
            .catch(err => console.log("network error: " + err))
            .then(response => {

                // read json from the response stream
                // and display the data
                response.json().then(data => {
                    document.location.reload();
                });
            })
    }

    function Copy() {
        if (switchBox.checked = true) {
            var text = document.getElementById('textBox').innerText;
        }
        if (switchBox.checked = false) {
            var text = document.getElementById('textBox').innerHTML;
        }

        navigator.clipboard.writeText(text).then(function () {
            if (switchBox.checked = true) {
                alert('�e�L�X�g���N���b�v�{�[�h�ɃR�s�[���܂����B');
            }
            if (switchBox.checked = false) {
                alert('HTML���N���b�v�{�[�h�ɃR�s�[���܂����B');
            }
        }, function (err) {
            alert('�R�s�[�G���[');
        });
    }


