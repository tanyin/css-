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
                    var tb1='';
                    var tb2='';
                    if(i < times) {
                        get_keyword(i + 1);
                    } else {
                        keywords = keywords.unique();
                        for(key in keywords) {
                            if(keywords.hasOwnProperty(key)) {
                                tb1 +="<tr><td>"+keywords[key][0]+"</td></tr>";
                                tb2 +="<tr><td>"+keywords[key][1]+"</td></tr>";
                               
                            }
                        }
                        
                        $('#tb1').html(tb1);
                        $('#tb2').html(tb2);
                        $('#content').hide();
                        $('#submit_content').hide();
                        $('#resulttable').show();
                        $('#tips, #refresh').show();
                        $('#submit_content').prop('disabled', false);

                        for(var j =0; j<$('#tb1').find('td').length; j++)
                        {
                            if(j&1)
                            {
                                $('#tb1').find('tr').eq(j).addClass('even');
                                $('#tb2').find('tr').eq(j).addClass('even');
                            }else
                            {
                                $('#tb1').find('tr').eq(j).addClass('odd');
                                $('#tb2').find('tr').eq(j).addClass('odd');
                            }
                            if($('#tb1').find('td').eq(j).height()>$('#tb2').find('td').eq(j).height())
                                $('#tb2').find('td').eq(j).height($('#tb1').find('td').eq(j).height());
                            else if($('#tb1').find('td').eq(j).height()<$('#tb2').find('td').eq(j).height())
                                $('#tb1').find('td').eq(j).height($('#tb2').find('td').eq(j).height());
                        }    
                        
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
        $('#resulttable').hide();
        
        
        $('#content').val('');
        $('#content').show();
        $('#content').focus();
        
        $('#submit_content').show();
        $('#tips, #refresh').hide();
    });
});