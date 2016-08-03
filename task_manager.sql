-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Aug 02, 2016 at 05:40 AM
-- Server version: 10.1.9-MariaDB
-- PHP Version: 5.6.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `task_manager`
--

-- --------------------------------------------------------

--
-- Table structure for table `friends`
--

CREATE TABLE `friends` (
  `user1` int(11) NOT NULL,
  `user2` int(11) NOT NULL,
  `message` text NOT NULL,
  `request_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `accept_time` timestamp NULL DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `friends`
--

INSERT INTO `friends` (`user1`, `user2`, `message`, `request_time`, `accept_time`, `is_active`) VALUES
(1, 2, 'lam ban tao di', '2016-01-11 13:59:54', NULL, 1),
(1, 3, 'tao la user3', '2016-01-11 14:03:21', NULL, 1),
(1, 4, '1234', '2016-01-20 17:57:28', NULL, 1),
(1, 5, 'tao la user5', '2016-01-11 14:03:21', NULL, 1),
(1, 6, '123', '2016-01-20 17:58:42', NULL, 1),
(1, 7, '', '2016-02-24 11:43:53', NULL, 1),
(1, 8, '123456', '2016-01-20 17:56:29', NULL, 1),
(2, 4, 'asdf', '2016-01-20 18:05:18', NULL, 1),
(2, 5, '12345', '2016-01-20 18:00:22', NULL, 1),
(10, 12, 'DKM', '2016-07-19 03:28:55', NULL, 1);

-- --------------------------------------------------------

--
-- Table structure for table `messages`
--

CREATE TABLE `messages` (
  `message_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `receive_id` int(11) DEFAULT NULL,
  `receive_name` varchar(255) DEFAULT NULL,
  `room_id` int(11) NOT NULL,
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `message` text NOT NULL,
  `notify` tinyint(1) NOT NULL DEFAULT '0',
  `message_format` varchar(4) NOT NULL DEFAULT 'html',
  `is_read` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `messages`
--

INSERT INTO `messages` (`message_id`, `user_id`, `user_name`, `receive_id`, `receive_name`, `room_id`, `create_time`, `is_active`, `message`, `notify`, `message_format`, `is_read`) VALUES
(8, 1, 'test1', 2, 'test2', 0, '2016-01-14 19:34:58', 1, 'hahah', 0, 'html', 0),
(9, 1, 'test1', 3, 'test3', 0, '2016-01-14 19:35:39', 1, 'Hohoho', 0, 'html', 0),
(10, 2, 'test2', 1, 'test1', 0, '2016-01-14 19:37:31', 1, 'uh', 0, 'html', 0),
(11, 1, 'test1', 0, '', 1, '2016-01-14 19:37:56', 1, 'bi lap lai la the d', 0, 'html', 0),
(12, 1, 'test1', 0, '', 1, '2016-01-14 19:39:44', 1, 'them tin nhan', 0, 'html', 0),
(13, 1, 'test1', 0, '', 1, '2016-01-14 19:39:55', 1, 'tinh hinh co gi moi khong', 0, 'html', 0),
(14, 1, 'test1', 0, '', 1, '2016-01-14 19:42:08', 1, 'vao nhom la co tin nhan', 0, 'html', 0),
(15, 1, 'test1', 0, '', 1, '2016-01-14 19:45:36', 1, 'thoi di ngu', 0, 'html', 0),
(16, 1, 'test1', 0, '', 1, '2016-01-18 12:53:54', 1, 'oi thoi roi', 0, 'html', 0),
(17, 1, 'test1', 0, '', 0, '2016-01-19 14:54:44', 1, 'aaaa', 0, 'html', 0),
(18, 1, 'test1', 0, '', 0, '2016-01-19 14:54:48', 1, 'a', 0, 'html', 0),
(19, 1, 'test1', 0, '', 1, '2016-01-19 14:55:14', 1, 'hehe', 0, 'html', 0),
(20, 1, 'test1', 0, '', 0, '2016-01-19 14:55:21', 1, 'ads', 0, 'html', 0),
(21, 1, 'test1', 0, '', 0, '2016-01-19 14:55:49', 1, 'hihi', 0, 'html', 0),
(22, 1, 'test1', 0, '', 0, '2016-01-19 14:55:55', 1, 'hihihi', 0, 'html', 0),
(23, 1, 'test1', 3, 'test3', 0, '2016-01-19 16:17:02', 1, 'huhu', 0, 'html', 0),
(24, 1, 'test1', 2, 'test2', 0, '2016-01-19 16:17:10', 1, 'hichic', 0, 'html', 0),
(25, 1, 'test1', 7, 'test7', 0, '2016-01-19 16:40:54', 1, 'hahah', 0, 'html', 0),
(26, 1, 'test1', 7, 'test7', 0, '2016-01-19 16:42:44', 1, 'lam sao cho het buon', 0, 'html', 0),
(27, 1, 'test1', 0, '', 0, '2016-01-20 14:50:38', 1, 'bolobala', 0, 'html', 0),
(28, 2, 'test2', 5, 'test5', 0, '2016-01-20 18:00:40', 1, 'the deo nao', 0, 'html', 0),
(34, 1, 'test1', 8, '', 0, '2016-01-26 13:10:00', 1, 'room_invitation:15', 1, 'text', 0),
(35, 1, 'test1', 2, 'test2', 0, '2016-01-27 12:51:58', 1, 'nao minh cung chat', 0, 'html', 0),
(36, 1, 'test1', 2, 'test2', 0, '2016-01-27 12:59:21', 1, 'adc', 0, 'html', 0),
(37, 1, 'test1', 2, 'test2', 0, '2016-01-27 12:59:35', 1, 'cung ngon day chu nhi', 0, 'html', 0),
(38, 1, 'test1', 2, 'test2', 0, '2016-01-27 13:01:33', 1, 'test phat', 0, 'html', 0),
(39, 1, 'test1', 2, 'test2', 0, '2016-01-27 13:01:40', 1, 'dech cha lua', 0, 'html', 0),
(40, 1, 'test1', 2, 'test2', 0, '2016-01-27 13:02:08', 1, 'aaaaaaaaaaaaaaaaaaa thu mot tin nhan dai that la dai xem may co ra dcuo khong', 0, 'html', 0),
(41, 1, 'test1', 2, 'test2', 0, '2016-01-27 13:06:08', 1, 'tiep tuc nhan tin xem co gi hot', 0, 'html', 0),
(42, 0, '', 2, 'test2', 0, '2016-01-27 17:58:16', 1, 'asf', 0, 'html', 0),
(44, 1, 'test1', 0, '', 20, '2016-02-21 04:16:40', 1, 'Hey', 0, 'html', 0),
(45, 1, 'test1', 0, '', 31, '2016-02-22 12:44:50', 1, 'Hi', 0, 'html', 0),
(46, 1, 'test1', 2, 'test2', 0, '2016-02-22 12:46:25', 1, 'Hello', 0, 'html', 0),
(47, 1, 'test1', 2, 'test2', 0, '2016-02-22 12:46:26', 1, 'HI', 0, 'html', 0),
(48, 1, 'test1', 2, 'test2', 0, '2016-02-22 12:46:30', 1, 'Hi', 0, 'html', 0),
(49, 2, 'test2', 1, 'test1', 0, '2016-02-22 12:46:35', 1, 'alo', 0, 'html', 0),
(50, 1, 'test1', 0, '', 31, '2016-02-22 12:46:47', 1, 'Hi', 0, 'html', 0),
(51, 1, 'test1', 0, '', 31, '2016-02-22 12:46:50', 1, 'asd', 0, 'html', 0),
(52, 1, 'test1', 0, '', 31, '2016-02-22 12:46:55', 1, 'Hi', 0, 'html', 0),
(53, 1, 'test1', 0, '', 31, '2016-02-22 12:47:07', 1, 'Hello', 0, 'html', 0),
(54, 2, 'test2', 0, '', 31, '2016-02-22 12:47:12', 1, 'alo', 0, 'html', 0),
(55, 1, 'test1', 0, '', 31, '2016-02-22 12:47:14', 1, '1', 0, 'html', 0),
(56, 1, 'test1', 0, '', 31, '2016-02-22 12:47:15', 1, '2', 0, 'html', 0),
(57, 1, 'test1', 0, '', 31, '2016-02-22 12:47:16', 1, '3', 0, 'html', 0),
(58, 1, 'test1', 0, '', 0, '2016-02-29 20:22:50', 1, 'aloo', 0, 'html', 0),
(59, 3, 'test3', 0, '', 0, '2016-02-29 20:22:51', 1, 'hi', 0, 'html', 0),
(60, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:06', 1, '2qqwewe', 0, 'html', 0),
(61, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:07', 1, 'aloo', 0, 'html', 0),
(62, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:11', 1, 'aloo', 0, 'html', 0),
(63, 1, 'test1', 3, 'test3', 0, '2016-02-29 20:23:14', 1, 'alo', 0, 'html', 0),
(64, 2, 'test2', 0, '', 0, '2016-02-29 20:23:17', 1, 'Ãª', 0, 'html', 0),
(65, 1, 'test1', 3, 'test3', 0, '2016-02-29 20:23:21', 1, 'Ãª', 0, 'html', 0),
(66, 2, 'test2', 0, '', 0, '2016-02-29 20:23:30', 1, 'vÃ o Ä‘Ãª', 0, 'html', 0),
(67, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:37', 1, 'báº­t aoe Ä‘i', 0, 'html', 0),
(68, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:46', 1, 'xem Ä‘c ko', 0, 'html', 0),
(69, 3, 'test3', 0, '', 0, '2016-02-29 20:23:51', 1, 'Ä‘á»£i tÃ½', 0, 'html', 0),
(70, 2, 'test2', 0, '', 0, '2016-02-29 20:41:35', 1, 'rá»“i lÃ m gÃ¬ ná»¯a anh', 0, 'html', 0),
(71, 1, 'test1', 0, '', 0, '2016-02-29 20:41:51', 1, 'CÃ³ ai vÃ o Ä‘c chÆ°a :D', 0, 'html', 0),
(72, 2, 'test2', 0, '', 0, '2016-02-29 20:42:24', 1, 'vÃ o cÃ¡i j tháº¿ a =))', 0, 'html', 0),
(73, 2, 'test2', 0, '', 0, '2016-02-29 20:55:51', 1, 'abc', 0, 'html', 0),
(74, 1, 'test1', 0, '', 33, '2016-03-03 06:23:02', 1, 'hey join room', 0, 'html', 0),
(75, 1, 'test1', 0, '', 33, '2016-03-03 06:23:07', 1, 'let play', 0, 'html', 0),
(76, 1, 'test1', 0, '', 0, '2016-03-07 00:26:31', 1, 'Ãª', 0, 'html', 0),
(77, 1, 'test1', 0, '', 0, '2016-03-07 00:26:35', 1, 'Ã´', 0, 'html', 0),
(78, 1, 'test1', 0, '', 33, '2016-03-07 00:27:13', 1, 'hey', 0, 'html', 0),
(79, 1, 'test1', 0, '', 0, '2016-03-07 00:28:03', 1, 'ssss', 0, 'html', 0),
(80, 1, 'test1', 0, '', 0, '2016-03-07 00:35:32', 1, 'room_invitation:35', 1, 'text', 0),
(81, 3, 'test3', 0, '', 0, '2016-03-07 00:35:33', 1, 'xxxxxxxxxx', 0, 'html', 0),
(82, 1, 'test1', 0, '', 0, '2016-03-07 00:35:35', 1, 'room_invitation:35', 1, 'text', 0),
(83, 1, 'test1', 0, '', 0, '2016-03-07 00:35:35', 1, 'room_invitation:35', 1, 'text', 0),
(84, 1, 'test1', 0, '', 0, '2016-03-07 00:35:35', 1, 'room_invitation:35', 1, 'text', 0),
(85, 4, 'test4', 0, '', 0, '2016-03-14 02:53:06', 1, 'E', 0, 'html', 0),
(86, 4, 'test4', 0, '', 0, '2016-03-14 02:53:10', 1, 'Ngon ko', 0, 'html', 0),
(87, 1, 'test1', 0, '', 36, '2016-03-14 02:55:28', 1, 'hello', 0, 'html', 0),
(88, 4, 'test4', 1, 'test1', 0, '2016-03-14 08:33:28', 1, 'hh', 0, 'html', 0),
(89, 1, 'test1', 4, 'test4', 0, '2016-03-14 08:33:39', 1, 'hey', 0, 'html', 0),
(90, 4, 'test4', 0, '', 0, '2016-03-18 04:22:55', 1, 'vao de', 0, 'html', 0),
(91, 4, 'test4', 0, '', 0, '2016-03-18 04:23:52', 1, '143', 0, 'html', 0),
(92, 1, 'test1', 0, '', 0, '2016-03-22 09:13:19', 1, 'Hello anh em', 0, 'html', 0),
(93, 3, 'test3', 0, '', 0, '2016-03-22 09:18:13', 1, 'aaaaa', 0, 'html', 0),
(94, 4, 'test4', 0, '', 0, '2016-03-22 14:57:19', 1, 'alo', 0, 'html', 0),
(95, 3, 'test3', 0, '', 0, '2016-03-24 12:50:33', 1, 'alo', 0, 'html', 0),
(96, 3, 'test3', 0, '', 0, '2016-03-24 12:50:35', 1, 'alo', 0, 'html', 0),
(97, 3, 'test3', 0, '', 0, '2016-03-24 12:50:38', 1, 'alo', 0, 'html', 0),
(98, 3, 'test3', 0, '', 0, '2016-03-29 09:31:00', 1, 'Ãª', 0, 'html', 0),
(99, 3, 'test3', 0, '', 0, '2016-03-31 07:43:46', 1, 'test 3 goi test 1 nghe ro tra loi', 0, 'html', 0),
(100, 5, 'test5', 0, '', 0, '2016-03-31 09:17:09', 1, 'ok', 0, 'html', 0),
(101, 6, 'test6', 0, '', 0, '2016-03-31 10:16:59', 1, 'a', 0, 'html', 0),
(102, 5, 'test5', 0, '', 46, '2016-04-01 04:12:16', 1, 'hihi', 0, 'html', 0),
(103, 5, 'test5', 0, '', 45, '2016-04-04 09:48:19', 1, 'Alo', 0, 'html', 0),
(104, 6, 'test6', 0, '', 0, '2016-04-06 09:07:12', 1, 'a', 0, 'html', 0),
(105, 6, 'test6', 0, '', 0, '2016-04-06 09:07:13', 1, 'a', 0, 'html', 0),
(106, 6, 'test6', 0, '', 0, '2016-04-06 09:07:14', 1, 'a', 0, 'html', 0),
(107, 6, 'test6', 0, '', 45, '2016-04-06 09:07:46', 1, 'a', 0, 'html', 0),
(108, 6, 'test6', 0, '', 45, '2016-04-06 09:07:47', 1, 'a', 0, 'html', 0),
(109, 11, 'kimhc0210', 0, '', 45, '2016-04-12 09:34:02', 1, 'c lo', 0, 'html', 0),
(110, 10, 'tranvutuan', 0, '', 0, '2016-05-10 09:13:26', 1, 'TÃ¬nh hÃ¬nh sao', 0, 'html', 0),
(111, 14, 'qthai2502', 0, '', 0, '2016-05-10 09:13:57', 1, 'ko vao dc', 0, 'html', 0),
(112, 14, 'qthai2502', 0, '', 0, '2016-05-10 09:14:07', 1, '=)))', 0, 'html', 0),
(113, 10, 'tranvutuan', 0, '', 56, '2016-05-10 09:14:31', 1, 'KIM AN Lá»’N', 0, 'html', 0),
(114, 16, 'wantlose', 0, '', 56, '2016-05-10 09:17:39', 1, '@@', 0, 'html', 0),
(115, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:34', 1, 'a', 0, 'html', 0),
(116, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:35', 1, 'a', 0, 'html', 0),
(117, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:36', 1, 'a', 0, 'html', 0),
(118, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:36', 1, 'a', 0, 'html', 0),
(119, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:36', 1, 'a', 0, 'html', 0),
(120, 16, 'wantlose', 0, '', 66, '2016-05-23 09:25:06', 1, 'ae Æ¡i', 0, 'html', 0),
(121, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:24', 1, 'a', 0, 'html', 0),
(122, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:25', 1, 'a', 0, 'html', 0),
(123, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:29', 1, 'a', 0, 'html', 0),
(124, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:29', 1, 'a', 0, 'html', 0),
(125, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:30', 1, 'a', 0, 'html', 0),
(126, 14, 'qthai2502', 0, '', 66, '2016-05-23 09:26:32', 1, 'vba', 0, 'html', 0),
(127, 14, 'qthai2502', 0, '', 0, '2016-06-09 09:07:36', 1, 'h', 0, 'html', 0),
(128, 18, 'kakashilfc', 0, '', 0, '2016-06-09 09:25:00', 1, 'ldslafdsa', 0, 'html', 0),
(129, 18, 'kakashilfc', 0, '', 83, '2016-06-09 09:25:17', 1, 'dsfadf', 0, 'html', 0),
(130, 11, 'kimhc0210', 0, '', 83, '2016-06-09 09:48:26', 1, 'fuck', 0, 'html', 0),
(131, 10, 'tranvutuan', 0, '', 85, '2016-06-09 10:02:58', 1, 'Cho KÃ­m', 0, 'html', 0),
(132, 14, 'qthai2502', 0, '', 85, '2016-06-09 10:03:00', 1, 'a', 0, 'html', 0),
(133, 14, 'qthai2502', 0, '', 85, '2016-06-09 10:03:01', 1, 's', 0, 'html', 0),
(134, 18, 'kakashilfc', 0, '', 85, '2016-06-09 10:03:29', 1, 'fdsafdsa', 0, 'html', 0),
(135, 19, 'angocbkhn', 0, '', 85, '2016-06-09 10:11:50', 1, 'co ai ko', 0, 'html', 0),
(136, 11, 'kimhc0210', 0, '', 85, '2016-06-10 01:28:28', 1, 'hello', 0, 'html', 0),
(137, 10, 'tranvutuan', 0, '', 97, '2016-06-15 09:25:15', 1, 'KIM AN LOL', 0, 'html', 0),
(138, 19, 'angocbkhn', 0, '', 97, '2016-06-15 09:25:42', 1, 'sao ko co ip nhi', 0, 'html', 0),
(139, 19, 'angocbkhn', 0, '', 97, '2016-06-15 09:25:51', 1, 'alo', 0, 'html', 0),
(140, 19, 'angocbkhn', 0, '', 97, '2016-06-15 09:25:55', 1, 'alo', 0, 'html', 0),
(141, 10, 'tranvutuan', 0, '', 97, '2016-06-17 09:41:46', 1, 'Kim an lol', 0, 'html', 0),
(142, 10, 'tranvutuan', 0, '', 106, '2016-07-08 09:03:57', 1, 'KIm Äƒn lÃ´l', 0, 'html', 0),
(143, 10, 'tranvutuan', 0, '', 106, '2016-07-08 09:04:16', 1, 'Kim máº·t lol', 0, 'html', 0),
(144, 17, 'duongdt', 0, '', 0, '2016-07-08 09:08:13', 1, 'ALo', 0, 'html', 0),
(145, 16, 'wantlose', 0, '', 0, '2016-07-13 09:31:59', 1, 'a', 0, 'html', 0),
(146, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:03', 1, 'dgsd', 0, 'html', 0),
(147, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:03', 1, 'g', 0, 'html', 0),
(148, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:11', 1, 'dg', 0, 'html', 0),
(149, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:11', 1, 'sd', 0, 'html', 0),
(150, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:12', 1, 'f', 0, 'html', 0),
(151, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:12', 1, 'sd', 0, 'html', 0),
(152, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:12', 1, 'sf', 0, 'html', 0),
(153, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:13', 1, 's', 0, 'html', 0),
(154, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:13', 1, 'fsd', 0, 'html', 0),
(155, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:13', 1, 'fs', 0, 'html', 0),
(156, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:14', 1, 'f', 0, 'html', 0),
(157, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:14', 1, 'sf', 0, 'html', 0),
(158, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:14', 1, 's', 0, 'html', 0),
(159, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:15', 1, 'fs', 0, 'html', 0),
(160, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:48:20', 1, 'alo', 0, 'html', 0),
(161, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:48:32', 1, 'cÃ³ ai khÃ´ng', 0, 'html', 0),
(162, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:48:46', 1, 'chÃ©m ko?', 0, 'html', 0),
(163, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:49:05', 1, '` hello', 0, 'html', 0),
(164, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:49:07', 1, 'hello', 0, 'html', 0),
(165, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:49:17', 1, '13h30', 0, 'html', 0),
(166, 20, 'batvuonggia', 0, '', 0, '2016-07-19 06:53:44', 1, 'd', 0, 'html', 0),
(167, 20, 'batvuonggia', 0, '', 112, '2016-07-22 05:31:32', 1, 'alo', 0, 'html', 0),
(168, 16, 'wantlose', 0, '', 112, '2016-07-25 09:28:00', 1, 'a', 0, 'html', 0),
(169, 16, 'wantlose', 0, '', 112, '2016-07-25 09:28:01', 1, 'a', 0, 'html', 0),
(170, 16, 'wantlose', 0, '', 112, '2016-07-25 09:28:03', 1, 'aa', 0, 'html', 0),
(171, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:25', 1, '2', 0, 'html', 0),
(172, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:30', 1, '1', 0, 'html', 0),
(173, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:31', 1, '3', 0, 'html', 0),
(174, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:31', 1, '5', 0, 'html', 0),
(175, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:37', 1, '8', 0, 'html', 0),
(176, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:38', 1, '-', 0, 'html', 0);

-- --------------------------------------------------------

--
-- Table structure for table `rooms`
--

CREATE TABLE `rooms` (
  `room_id` int(11) NOT NULL,
  `room_name` varchar(255) NOT NULL,
  `server_id` int(11) NOT NULL,
  `host_ip` varchar(255) NOT NULL,
  `create_user` int(11) NOT NULL,
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `last_active` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `rooms`
--

INSERT INTO `rooms` (`room_id`, `room_name`, `server_id`, `host_ip`, `create_user`, `create_time`, `is_active`, `last_active`) VALUES
(114, 'Hnay VÃ o Ä‘Ã¢y', 3, '103.56.157.252:5556', 17, '2016-08-01 08:50:38', 1, NULL),
(115, 'CNET', 4, '103.56.157.252:443', 14, '2016-08-01 14:39:19', 1, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `room_users`
--

CREATE TABLE `room_users` (
  `room_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `user_lan_ip` varchar(255) NOT NULL,
  `join_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `last_active` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `room_users`
--

INSERT INTO `room_users` (`room_id`, `user_id`, `user_name`, `user_lan_ip`, `join_time`, `is_active`, `last_active`) VALUES
(61, 13, 'dinhtan89', '192.168.60.10', '2016-05-17 13:56:43', 1, '0000-00-00 00:00:00'),
(67, 13, 'dinhtan89', '', '2016-05-27 09:40:48', 1, '0000-00-00 00:00:00'),
(97, 14, 'qthai2502', '192.168.40.13', '2016-06-24 10:09:52', 1, '0000-00-00 00:00:00'),
(101, 16, 'wantlose', '192.168.60.12', '2016-06-27 09:45:12', 1, '0000-00-00 00:00:00'),
(103, 12, 'CyberZ', '192.168.60.10', '2016-07-01 10:02:12', 1, '0000-00-00 00:00:00'),
(105, 17, 'duongdt', '', '2016-07-07 10:08:40', 1, '0000-00-00 00:00:00'),
(107, 14, 'qthai2502', '', '2016-07-08 09:08:06', 1, '0000-00-00 00:00:00'),
(108, 10, 'tranvutuan', '192.168.50.12', '2016-07-08 09:09:59', 1, '0000-00-00 00:00:00'),
(112, 11, 'kimhc0210', '192.168.60.17', '2016-07-28 09:20:11', 1, '0000-00-00 00:00:00'),
(114, 10, 'tranvutuan', '192.168.60.11', '2016-08-01 09:37:26', 1, '0000-00-00 00:00:00'),
(114, 11, 'kimhc0210', '192.168.60.14', '2016-08-01 09:36:36', 1, '0000-00-00 00:00:00'),
(114, 12, 'CyberZ', '192.168.60.12', '2016-08-01 09:36:33', 1, '0000-00-00 00:00:00'),
(114, 16, 'wantlose', '192.168.60.17', '2016-08-01 09:39:06', 1, '0000-00-00 00:00:00'),
(114, 17, 'duongdt', '192.168.60.10', '2016-08-01 09:36:40', 1, '0000-00-00 00:00:00'),
(115, 14, 'qthai2502', '192.168.40.11', '2016-08-01 14:40:58', 1, '0000-00-00 00:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `servers`
--

CREATE TABLE `servers` (
  `server_id` int(11) NOT NULL,
  `host` varchar(50) NOT NULL,
  `port` int(11) NOT NULL,
  `hub` varchar(255) NOT NULL,
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `number_connected` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `servers`
--

INSERT INTO `servers` (`server_id`, `host`, `port`, `hub`, `create_time`, `is_active`, `number_connected`) VALUES
(2, '103.56.157.252', 992, 'aoe_vpn_hub', '2016-01-25 17:14:46', 1, 1),
(3, '103.56.157.252', 5556, 'aoe_vpn_hub2', '2016-01-25 17:14:46', 1, 1),
(4, '103.56.157.252', 443, 'aoe_vpn_hub3', '2016-01-25 17:16:11', 1, 1),
(5, '103.56.157.252', 5555, 'aoe_vpn_hub4', '2016-01-25 17:16:11', 1, 0),
(6, '103.56.157.252', 5556, 'aoe_vpn_hub5', '2016-01-25 17:16:11', 1, 0),
(7, '103.56.157.252', 5557, 'aoe_vpn_hub6', '2016-01-25 17:16:11', 1, 0),
(8, '103.56.157.252', 5558, 'aoe_vpn_hub7', '2016-01-25 17:16:11', 1, 0),
(9, '103.56.157.252', 5559, 'aoe_vpn_hub8', '2016-01-25 17:16:11', 1, 0),
(10, '103.56.157.252', 5560, 'aoe_vpn_hub9', '2016-01-25 17:16:11', 1, 0),
(11, '103.56.157.252', 5561, 'aoe_vpn_hub10', '2016-01-25 17:16:11', 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `user_caches`
--

CREATE TABLE `user_caches` (
  `user_id` int(11) NOT NULL,
  `user_name` varchar(100) NOT NULL,
  `user_email` varchar(255) NOT NULL,
  `password` varchar(100) NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `last_active` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `user_caches`
--

INSERT INTO `user_caches` (`user_id`, `user_name`, `user_email`, `password`, `is_active`, `last_active`) VALUES
(1, 'test1', '', 'e99a18c428cb38d5f260853678922e03', 1, '2016-04-11 16:29:24'),
(2, 'test2', '', 'e99a18c428cb38d5f260853678922e03', 1, '2016-04-04 10:27:28'),
(3, 'test3', '', 'e99a18c428cb38d5f260853678922e03', 1, '2016-04-07 10:25:32'),
(4, 'test4', '', 'e99a18c428cb38d5f260853678922e03', 1, '2016-04-01 09:29:23'),
(5, 'test5', '', 'e99a18c428cb38d5f260853678922e03', 1, '2016-04-07 10:25:24'),
(6, 'test6', '', 'e99a18c428cb38d5f260853678922e03', 1, '2016-04-07 10:25:20'),
(7, 'test7', '', 'e99a18c428cb38d5f260853678922e03', 1, '2016-03-31 09:19:32'),
(8, 'test8', '', 'e99a18c428cb38d5f260853678922e03', 1, '2016-04-07 10:25:45'),
(9, 'admintest1', '', 'Admin12345678', 1, '2016-07-28 14:46:53'),
(10, 'tranvutuan', '', 'Tuan@123', 1, '2016-08-01 10:26:30'),
(11, 'kimhc0210', '', 'A123987@@', 1, '2016-08-01 10:26:57'),
(12, 'CyberZ', '', 'Zone@$132', 1, '2016-08-01 10:26:31'),
(13, 'dinhtan89', '', 'Dinhtan89', 1, '2016-08-01 16:05:14'),
(14, 'qthai2502', '', 'ABC123456789', 1, '2016-08-01 15:58:50'),
(15, 'duongdt49mt221189', '', 'D07u4ndu0n6', 1, '2016-04-13 10:14:09'),
(16, 'wantlose', '', 'Duong168', 1, '2016-08-01 10:26:39'),
(17, 'duongdt', '', 'D07u4ndu0n6', 1, '2016-08-01 10:28:37'),
(18, 'kakashilfc', '', 'Kakashi123@', 1, '2016-08-01 10:27:41'),
(19, 'angocbkhn', '', 'Ngoc12a3', 1, '2016-08-01 10:27:14'),
(20, 'batvuonggia', '', 'Loveofromeo@@123', 1, '2016-07-22 07:33:55'),
(21, 'nguycongtu', '', 'Vxn6bbx5', 1, '2016-08-01 10:10:13');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `friends`
--
ALTER TABLE `friends`
  ADD PRIMARY KEY (`user1`,`user2`);

--
-- Indexes for table `messages`
--
ALTER TABLE `messages`
  ADD PRIMARY KEY (`message_id`);

--
-- Indexes for table `rooms`
--
ALTER TABLE `rooms`
  ADD PRIMARY KEY (`room_id`);

--
-- Indexes for table `room_users`
--
ALTER TABLE `room_users`
  ADD PRIMARY KEY (`room_id`,`user_id`);

--
-- Indexes for table `servers`
--
ALTER TABLE `servers`
  ADD PRIMARY KEY (`server_id`);

--
-- Indexes for table `user_caches`
--
ALTER TABLE `user_caches`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `messages`
--
ALTER TABLE `messages`
  MODIFY `message_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=177;
--
-- AUTO_INCREMENT for table `rooms`
--
ALTER TABLE `rooms`
  MODIFY `room_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=116;
--
-- AUTO_INCREMENT for table `servers`
--
ALTER TABLE `servers`
  MODIFY `server_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT for table `user_caches`
--
ALTER TABLE `user_caches`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
