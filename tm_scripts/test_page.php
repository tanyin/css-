<!DOCTYPE html>
<!--

      . ;  .,     . ,                        . .        
     s r@ :,5@   ;.H@     X3XXi ,hS2SS#@;,@ ,r @  ,s,X, 
    ,r @, :.;3   s G:.:.;  rs2& @hAhBsr:; @ .; @  ;; #2 
    S #@rXX  XXh@ ;&9s H@;   : @B  , ,  ; @ .i;@  ,A;@; 
   i  M .:5,i@.ss @r i @  :.,  A ,rA2sX.22@  B&@  .B;@, 
  i X M   . r2 :,:r: .rM  i#@@@#@@H9h9@@SS@  hX@   G:@. 
  3@@ #.;:sssr B#H.Ar,@:       :s@.   ,.Si@  92@   9:@  
   .: @:r.@rrh,@  h#h3@    Xissi;52siss 5i@  32@   9:@  
    r.#,rs@  :S@. ii;@     X@@##;A@BMB@ 2i@  32@   h2@  
    s:@.ih@  ;s@  rrsH         .r@;     r@M  32@   ,@S  
    ii@ S2Bii5r@i;3@&iAr       ;;S22B@@@     9S@  r2:Sr 
    Sr@  3#@@B@@s@@  SBHB,&@@@@@@@@@A5;  S2srrM@  :BM@B 
    ;@@        X@9    :A@; &Gi:.         2@@@@@,    ;,  

-->
<html>
<head>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
<meta name="robots" content="noindex,noarchive,nofollow" />
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
<title>$content</title>
</head>
<body>
<div style="display: none;"><?php echo $content; ?></div>
<script type="text/javascript" src="http://static.zuodao.com/js/static.js?v=998"></script>
<script type="text/javascript">
var array = $('div').text().split("\n");
function doPost(i) {
    if(i >= array.length - 1) {
        return;
    }
    var temp_array = eval(array[i]);
    if(typeof temp_array[0] === 'undefined') {
        doPost(i+1);
        return;
    }
    var data = {
        source: temp_array[0][0][1],
    }
    var target_array = [];
    for(var j in temp_array[1]) {
        for(var x in temp_array[1][j][1]) {
            target_array.push(temp_array[1][j][1][x]);
        }
    }
    for(var m in temp_array[5]) {
        for(var n in temp_array[5][m][2]) {
            target_array.push(temp_array[5][m][2][n][0]);
        }
    }
    console.log(i + ':' + data.source);
    data.target = target_array;
    $.post('/test/store_words', data, function(){
        doPost(i+1);
    });
}
</script>
</body>
</html>
