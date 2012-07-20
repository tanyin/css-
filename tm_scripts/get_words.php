<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');
header('Content-Type: text/html; charset=UTF-8');
class Get_words extends CI_Controller {

    function __construct() {
        parent::__construct();
        set_time_limit(300);
    }
    function run($num)
    {
        echo $num, ':';
        $start_time = time();
        $fp = fopen('../uploads/temp/output_' . $num . '.txt', 'w');
        //$words = $this->get_words_from_file();
        $step = 1000;
        $start = $num * $step;
        $words = $this->get_words(1, $start, $step);
        for($i = 0; $i < count($words); $i ++)
        {
            //$result = iconv('GBK', 'UTF-8', $this->get_word_translation($words[$i]['word'], 2, 1));
            //$result = iconv('GBK', 'UTF-8', $this->get_word_translation($words[$i]['word'], 2, 1));
            if(strstr($words[$i]['word'], ' '))
            {
                continue;
            }
            $result = $this->get_word_translation($words[$i]['word'], 1, 2);
            if(strpos($result, 'Python Server Fetch Info'))
            {
                continue;
            }
            //echo $words[$i]['word'], '<br />';
            //echo $result, '<br />', '<br />';
            //echo iconv_get_encoding($result);
            fwrite($fp, $result."\n");
        }
        // for($i = $start; $i < $start + $step; $i++)
        // {
            // if(!isset($words[$i]))
            // {
                // break;
            // }
            // //$result = iconv('GBK', 'UTF-8', $this->get_word_translation(trim($words[$i]), 1, 2));
            // $result = $this->get_word_translation(trim($words[$i]), 2, 1);
            // if(strpos($result, 'Python Server Fetch Info'))
            // {
                // continue;
            // }
            // fwrite($fp, $result."\n");
        // }
        //fwrite($fp, 'xxx');
        fclose($fp);
        echo time() - $start_time;
        echo ';', date('H:i:s');
    }
    function get_words_from_file()
    {
        $fp = fopen('../uploads/input.dic', 'r'); 
        $content = fread($fp, filesize('../uploads/input.dic'));
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
            $table = 'memory_cn_word';
        }
        $sql = "SELECT `word` FROM `$table` WHERE `id` > 121293 ORDER BY `id` LIMIT $start, $step";
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
            curl_setopt($ch, CURLOPT_URL, 'http://translate.google.cn/translate_a/t?client=t&text='.$word.'&hl=en&sl=en&tl=zh-CN&multires=1&otf=1&ssel=3&tsel=6&sc=1');
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
