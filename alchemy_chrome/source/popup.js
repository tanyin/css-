$(function(){
    Array.prototype.unique = function() {
        var a =this.sort(function(x,y){return (x[0]==y[0])?(x[1].localeCompare(y[1])):(x[0].localeCompare(y[0]));});
        var result = [];
        var l = a.length;
        for(var i=0; i<l; i++) {
            for(var j=i+1; j<l; j++) {
                // If a[i][1] is found later in the array
                if (a[i][1] === a[j][1]) {
                    j = ++i;
                }
            }
            result.push(this[i]);
        }
        return result;
    };
    String.prototype.Blength = function() {
        var arr = this.match(/[^\x00-\xff]/ig);
        return arr === null ? this.length : this.length + arr.length;
    };
    $('#submit_content').on('click', function(){
        var content = $.trim($('#content').val());//textarea的内容
        var times = Math.ceil(content.length / 60000);//分割的次数
        var keywords = [];
        if(content === '') {
            return false;
        }
        $('#submit_content').prop('disabled', true);//让button不可用
        var get_keyword = function(i){
            var text = content.substr((i - 1) * 60000, 60000 + 100);
            $.ajax({
                //url: "http://access.alchemyapi.com/calls/text/TextGetRankedKeywords",
                url: "http://access.alchemyapi.com/calls/text/TextGetRankedNamedEntities",
                data: {
                    apikey: 'fcc05d8b66a88c8da4addc9960985475c28b0423',
                    text: text,
                    outputMode: 'json'
                    //jsonp: 'callback'
                },
                dataType: 'json',
                type: 'post',
                //jsonpCallback: 'callback',
                success: function(res){
                    var result = [];
                    var key;
                    for(key in res.entities) {
                        if(res.entities.hasOwnProperty(key)) {
                            //alert(res.entities[key].type);
                            result.push([res.entities[key].type,res.entities[key].text]);
                        }
                    }
                    keywords = keywords.concat(result);
                    result = '';

                    if(i < times) {
                        get_keyword(i + 1);
                    } else {
                        keywords = keywords.unique();
                        var if_start = true;
                        var tmp = '';
                        for(key in keywords) {
                            if(keywords.hasOwnProperty(key)) {
                                if(tmp!=keywords[key][0])//换下一组了
                                {    
                                    if(!if_start)//不是刚开始 所以需要先输出一个空白行
                                    result+='\n';
                                    result+=keywords[key][0]+':\n';
                                }
                                result+=keywords[key][1]+'\n';
                                if_start = false;
                                tmp=keywords[key][0];
                            }
                        }
                        
                        $('#content').val(result).focus();
                        $('#submit_content').hide();
                        $('#resulttable').show();
                        $('#tips, #refresh').show();
                        $('#submit_content').prop('disabled', false);

                          
                        
                    }
                },
                error: function() {
                    alert('Server is busy.');
                    $('#submit_content').prop('disabled', false);
                }
            });
        };
        get_keyword(1);
    });
    $('#refresh').on('click', function(){
         
        
        $('#content').val('');
        $('#content').focus();
        
        $('#submit_content').show();
        $('#tips, #refresh').hide();
    });
});