<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');
header('Content-Type: text/html; charset=UTF-8');
class Test_a extends CI_Controller {

    function __construct() {
        parent::__construct();
        set_time_limit(300);
    }
    function run($num)
    {
        echo $num, ':';
        $start_time = time();
        $fp = fopen('../uploads/temp/output_' . $num . '.txt', 'w');
        $words = $this->get_words_from_file();
        $step = 100;
        $start = $num * $step;
        // $words = $this->get_words(2, $start, $step);
        // for($i = 0; $i < count($words); $i ++)
        // {
            // //$result = iconv('GBK', 'UTF-8', $this->get_word_translation($words[$i]['word'], 2, 1));
            // $result = $this->get_word_translation($words[$i]['word'], 2, 1);
            // if(strpos($result, 'Python Server Fetch Info'))
            // {
                // continue;
            // }
            // //echo $words[$i]['word'], '<br />';
            // //echo $result, '<br />', '<br />';
            // //echo iconv_get_encoding($result);
            // fwrite($fp, $result."\n");
        // }
        for($i = $start; $i < $start + $step; $i++)
        {
            if(!isset($words[$i]))
            {
                break;
            }
            //$result = iconv('GBK', 'UTF-8', $this->get_word_translation(trim($words[$i]), 1, 2));
            $result = $this->get_word_translation(trim($words[$i]), 1, 2);
            if(strpos($result, 'Python Server Fetch Info'))
            {
                continue;
            }
            //echo $result, '<br />';
            fwrite($fp, $result."\n");
        }
        //fwrite($fp, 'xxx');
        fclose($fp);
        echo time() - $start_time;
        echo ';', date('H:i:s');
    }
    function chinese_tokenizer()
    {
        $text = "这是一套基于词频词典的机械中文分词引擎，它能将一整段的汉字基本正确的切分成词。词是汉语的基本语素单位，而书写的时候不像英语会在词之间用空格分开，所以如何准确而又快速的分词一直是中文分词的攻关难点。";
        $text = iconv("UTF-8", "GBK//IGNORE", $text);
        $text = urlencode($text);
        $opts = array(
          'http'=>array(
            'method'=>"POST",
            'header'=>"Content-type: application/x-www-form-urlencoded\r\n".
                      "Content-length:".strlen($text)."\r\n" .
                      "Cookie: foo=bar\r\n" .
                      "\r\n",
            'content' => $text,
          )
        );
        $context = stream_context_create($opts);
        $result = file_get_contents("http://127.0.0.1:1985", false, $context);
        $result = iconv("GBK", "UTF-8//IGNORE", $result);
        echo $result;
    }
    function get_words_from_file()
    {
        $fp = fopen('../uploads/phrase.txt', 'r'); 
        $content = fread($fp, filesize('../uploads/phrase.txt'));
        //print_r(explode("\n", $content));
        fclose($fp);
        return explode("\n", $content);
    }
    function get_words($language, $start, $step)
    {
        if($language === 1)
        {
            $table = 'memory_en_word';
        }
        else
        {
            $table = 'temp_memory_cn_word';
        }
        $sql = "SELECT `word` FROM `$table` ORDER BY `id` LIMIT $start, $step";
        return $this->db->query($sql)->result_array();
    }
    
    function get_word_translation($word, $source_lang, $target_lang)
    {
        $ch = curl_init();
        //set the url, number of POST vars, POST data
        curl_setopt($ch, CURLOPT_ENCODING, 'UTF-8');
        curl_setopt($ch, CURLOPT_PROXY, "192.168.1.183:8087");
        $header[] = 'User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.83 Safari/535.11';
        curl_setopt($ch, CURLOPT_HTTPHEADER, $header);
        if($source_lang === 1)
        {
            curl_setopt($ch, CURLOPT_URL, 'http://translate.google.cn/translate_a/t?client=t&text='.urlencode($word).'&hl=en&sl=en&tl=zh-CN&multires=1&otf=1&ssel=3&tsel=6&sc=1');
        }
        else
        {
            curl_setopt($ch, CURLOPT_URL, 'http://translate.google.cn/translate_a/t?client=t&text='.urlencode($word).'&hl=en&sl=zh-CN&tl=en&multires=1&otf=1&pc=1&ssel=0&tsel=0&sc=1');
        }
        //echo 'http://translate.google.cn/translate_a/t?client=t&text='.urlencode($word).'&hl=en&sl=zh-CN&tl=en&multires=1&otf=1&pc=1&ssel=0&tsel=0&sc=1', '<br />';
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, TRUE);
        curl_setopt($ch, CURLOPT_CONNECTTIMEOUT, 5); 
        //curl_setopt($ch, CURLOPT_POSTFIELDS, $post_data);
        //execute post http://translate.google.cn/translate_a/t?client=t&text=%E6%B5%8B%E8%AF%95&hl=en&sl=zh-CN&tl=en&multires=1&otf=1&pc=1&ssel=0&tsel=0&sc=1
        $result = curl_exec($ch);
        //close connection 
        curl_close($ch);
        return $result;
    }
}
