$(function(){
    Array.prototype.unique = function() {
        var a = [];
        var l = this.length;
        for(var i=0; i<l; i++) {
            for(var j=i+1; j<l; j++) {
                // If this[i] is found later in the array
                if (this[i] === this[j]) {
                    j = ++i;
                }
            }
            a.push(this[i]);
        }
        return a;
    };
    String.prototype.Blength = function() {
        var arr = this.match(/[^\x00-\xff]/ig);
        return arr === null ? this.length : this.length + arr.length;
    };
    $('#submit_content').on('click', function(){
        var content = $.trim($('#content').val());
        var times = Math.ceil(content.length / 6000);
        var keywords = [];
        if(content === '') {
            return false;
        }
        $('#submit_content').prop('disabled', true);
        var get_keyword = function(i){
            var text = content.substr((i - 1) * 6000, 6000 + 100);
            $.ajax({
                url: "http://access.alchemyapi.com/calls/text/TextGetRankedKeywords",
                data: {
                    apikey: 'fcc05d8b66a88c8da4addc9960985475c28b0423',
                    text: text,
                    outputMode: 'json',
                    jsonp: 'callback'
                },
                dataType: 'jsonp',
                jsonpCallback: 'callback',
                success: function(res){
                    var result = [];
                    var key;
                    for(key in res.keywords) {
                        if(res.keywords.hasOwnProperty(key)) {
                            result.push(res.keywords[key].text);
                        }
                    }
                    keywords = keywords.concat(result);
                    result = '';
                    if(i < times) {
                        get_keyword(i + 1);
                    } else {
                        keywords = keywords.unique();
                        for(key in keywords) {
                            if(keywords.hasOwnProperty(key)) {
                                result += keywords[key] + "\n";
                            }
                        }
                        $('#content').val(result).focus();
                        $('#submit_content').hide();
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
        $('#content').val('').focus();
        $('#submit_content').show();
        $('#tips, #refresh').hide();
    });
});