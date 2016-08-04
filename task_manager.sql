-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Aug 04, 2016 at 07:46 PM
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
  `request_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `type` tinyint(2) NOT NULL DEFAULT '0' COMMENT '-1=ignore,0=pend,1=friend',
  `ignore_type` tinyint(2) NOT NULL DEFAULT '1' COMMENT 'who want ignore? 1=user1,2=user2,3=both'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `friends`
--

INSERT INTO `friends` (`user1`, `user2`, `request_time`, `type`, `ignore_type`) VALUES
(9, 10, '2016-01-11 13:59:54', 1, 1),
(9, 11, '2016-01-11 14:03:21', 1, 1),
(9, 14, '2016-01-20 17:58:42', 0, 1),
(9, 15, '2016-02-24 11:43:53', -1, 1),
(10, 12, '2016-07-19 03:28:55', 0, 1),
(12, 9, '2016-01-20 17:57:28', 1, 1),
(13, 9, '2016-01-11 14:03:21', 0, 1),
(16, 9, '2016-01-20 17:56:29', -1, 1),
(17, 9, '2016-01-20 18:05:18', -1, 2),
(18, 9, '2016-01-20 18:00:22', -1, 3);

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
  `message` text NOT NULL,
  `notify` tinyint(1) NOT NULL DEFAULT '0',
  `is_read` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `messages`
--

INSERT INTO `messages` (`message_id`, `user_id`, `user_name`, `receive_id`, `receive_name`, `room_id`, `create_time`, `message`, `notify`, `is_read`) VALUES
(8, 1, 'test1', 2, 'test2', 0, '2016-01-14 19:34:58', 'hahah', 0, 0),
(9, 1, 'test1', 3, 'test3', 0, '2016-01-14 19:35:39', 'Hohoho', 0, 0),
(10, 2, 'test2', 1, 'test1', 0, '2016-01-14 19:37:31', 'uh', 0, 0),
(11, 1, 'test1', 0, '', 1, '2016-01-14 19:37:56', 'bi lap lai la the d', 0, 0),
(12, 1, 'test1', 0, '', 1, '2016-01-14 19:39:44', 'them tin nhan', 0, 0),
(13, 1, 'test1', 0, '', 1, '2016-01-14 19:39:55', 'tinh hinh co gi moi khong', 0, 0),
(14, 1, 'test1', 0, '', 1, '2016-01-14 19:42:08', 'vao nhom la co tin nhan', 0, 0),
(15, 1, 'test1', 0, '', 1, '2016-01-14 19:45:36', 'thoi di ngu', 0, 0),
(16, 1, 'test1', 0, '', 1, '2016-01-18 12:53:54', 'oi thoi roi', 0, 0),
(17, 1, 'test1', 0, '', 0, '2016-01-19 14:54:44', 'aaaa', 0, 0),
(18, 1, 'test1', 0, '', 0, '2016-01-19 14:54:48', 'a', 0, 0),
(19, 1, 'test1', 0, '', 1, '2016-01-19 14:55:14', 'hehe', 0, 0),
(20, 1, 'test1', 0, '', 0, '2016-01-19 14:55:21', 'ads', 0, 0),
(21, 1, 'test1', 0, '', 0, '2016-01-19 14:55:49', 'hihi', 0, 0),
(22, 1, 'test1', 0, '', 0, '2016-01-19 14:55:55', 'hihihi', 0, 0),
(23, 1, 'test1', 3, 'test3', 0, '2016-01-19 16:17:02', 'huhu', 0, 0),
(24, 1, 'test1', 2, 'test2', 0, '2016-01-19 16:17:10', 'hichic', 0, 0),
(25, 1, 'test1', 7, 'test7', 0, '2016-01-19 16:40:54', 'hahah', 0, 0),
(26, 1, 'test1', 7, 'test7', 0, '2016-01-19 16:42:44', 'lam sao cho het buon', 0, 0),
(27, 1, 'test1', 0, '', 0, '2016-01-20 14:50:38', 'bolobala', 0, 0),
(28, 2, 'test2', 5, 'test5', 0, '2016-01-20 18:00:40', 'the deo nao', 0, 0),
(34, 1, 'test1', 8, '', 0, '2016-01-26 13:10:00', 'room_invitation:15', 1, 0),
(35, 1, 'test1', 2, 'test2', 0, '2016-01-27 12:51:58', 'nao minh cung chat', 0, 0),
(36, 1, 'test1', 2, 'test2', 0, '2016-01-27 12:59:21', 'adc', 0, 0),
(37, 1, 'test1', 2, 'test2', 0, '2016-01-27 12:59:35', 'cung ngon day chu nhi', 0, 0),
(38, 1, 'test1', 2, 'test2', 0, '2016-01-27 13:01:33', 'test phat', 0, 0),
(39, 1, 'test1', 2, 'test2', 0, '2016-01-27 13:01:40', 'dech cha lua', 0, 0),
(40, 1, 'test1', 2, 'test2', 0, '2016-01-27 13:02:08', 'aaaaaaaaaaaaaaaaaaa thu mot tin nhan dai that la dai xem may co ra dcuo khong', 0, 0),
(41, 1, 'test1', 2, 'test2', 0, '2016-01-27 13:06:08', 'tiep tuc nhan tin xem co gi hot', 0, 0),
(42, 0, '', 2, 'test2', 0, '2016-01-27 17:58:16', 'asf', 0, 0),
(44, 1, 'test1', 0, '', 20, '2016-02-21 04:16:40', 'Hey', 0, 0),
(45, 1, 'test1', 0, '', 31, '2016-02-22 12:44:50', 'Hi', 0, 0),
(46, 1, 'test1', 2, 'test2', 0, '2016-02-22 12:46:25', 'Hello', 0, 0),
(47, 1, 'test1', 2, 'test2', 0, '2016-02-22 12:46:26', 'HI', 0, 0),
(48, 1, 'test1', 2, 'test2', 0, '2016-02-22 12:46:30', 'Hi', 0, 0),
(49, 2, 'test2', 1, 'test1', 0, '2016-02-22 12:46:35', 'alo', 0, 0),
(50, 1, 'test1', 0, '', 31, '2016-02-22 12:46:47', 'Hi', 0, 0),
(51, 1, 'test1', 0, '', 31, '2016-02-22 12:46:50', 'asd', 0, 0),
(52, 1, 'test1', 0, '', 31, '2016-02-22 12:46:55', 'Hi', 0, 0),
(53, 1, 'test1', 0, '', 31, '2016-02-22 12:47:07', 'Hello', 0, 0),
(54, 2, 'test2', 0, '', 31, '2016-02-22 12:47:12', 'alo', 0, 0),
(55, 1, 'test1', 0, '', 31, '2016-02-22 12:47:14', '1', 0, 0),
(56, 1, 'test1', 0, '', 31, '2016-02-22 12:47:15', '2', 0, 0),
(57, 1, 'test1', 0, '', 31, '2016-02-22 12:47:16', '3', 0, 0),
(58, 1, 'test1', 0, '', 0, '2016-02-29 20:22:50', 'aloo', 0, 0),
(59, 3, 'test3', 0, '', 0, '2016-02-29 20:22:51', 'hi', 0, 0),
(60, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:06', '2qqwewe', 0, 0),
(61, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:07', 'aloo', 0, 0),
(62, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:11', 'aloo', 0, 0),
(63, 1, 'test1', 3, 'test3', 0, '2016-02-29 20:23:14', 'alo', 0, 0),
(64, 2, 'test2', 0, '', 0, '2016-02-29 20:23:17', 'Ãª', 0, 0),
(65, 1, 'test1', 3, 'test3', 0, '2016-02-29 20:23:21', 'Ãª', 0, 0),
(66, 2, 'test2', 0, '', 0, '2016-02-29 20:23:30', 'vÃ o Ä‘Ãª', 0, 0),
(67, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:37', 'báº­t aoe Ä‘i', 0, 0),
(68, 3, 'test3', 1, 'test1', 0, '2016-02-29 20:23:46', 'xem Ä‘c ko', 0, 0),
(69, 3, 'test3', 0, '', 0, '2016-02-29 20:23:51', 'Ä‘á»£i tÃ½', 0, 0),
(70, 2, 'test2', 0, '', 0, '2016-02-29 20:41:35', 'rá»“i lÃ m gÃ¬ ná»¯a anh', 0, 0),
(71, 1, 'test1', 0, '', 0, '2016-02-29 20:41:51', 'CÃ³ ai vÃ o Ä‘c chÆ°a :D', 0, 0),
(72, 2, 'test2', 0, '', 0, '2016-02-29 20:42:24', 'vÃ o cÃ¡i j tháº¿ a =))', 0, 0),
(73, 2, 'test2', 0, '', 0, '2016-02-29 20:55:51', 'abc', 0, 0),
(74, 1, 'test1', 0, '', 33, '2016-03-03 06:23:02', 'hey join room', 0, 0),
(75, 1, 'test1', 0, '', 33, '2016-03-03 06:23:07', 'let play', 0, 0),
(76, 1, 'test1', 0, '', 0, '2016-03-07 00:26:31', 'Ãª', 0, 0),
(77, 1, 'test1', 0, '', 0, '2016-03-07 00:26:35', 'Ã´', 0, 0),
(78, 1, 'test1', 0, '', 33, '2016-03-07 00:27:13', 'hey', 0, 0),
(79, 1, 'test1', 0, '', 0, '2016-03-07 00:28:03', 'ssss', 0, 0),
(80, 1, 'test1', 0, '', 0, '2016-03-07 00:35:32', 'room_invitation:35', 1, 0),
(81, 3, 'test3', 0, '', 0, '2016-03-07 00:35:33', 'xxxxxxxxxx', 0, 0),
(82, 1, 'test1', 0, '', 0, '2016-03-07 00:35:35', 'room_invitation:35', 1, 0),
(83, 1, 'test1', 0, '', 0, '2016-03-07 00:35:35', 'room_invitation:35', 1, 0),
(84, 1, 'test1', 0, '', 0, '2016-03-07 00:35:35', 'room_invitation:35', 1, 0),
(85, 4, 'test4', 0, '', 0, '2016-03-14 02:53:06', 'E', 0, 0),
(86, 4, 'test4', 0, '', 0, '2016-03-14 02:53:10', 'Ngon ko', 0, 0),
(87, 1, 'test1', 0, '', 36, '2016-03-14 02:55:28', 'hello', 0, 0),
(88, 4, 'test4', 1, 'test1', 0, '2016-03-14 08:33:28', 'hh', 0, 0),
(89, 1, 'test1', 4, 'test4', 0, '2016-03-14 08:33:39', 'hey', 0, 0),
(90, 4, 'test4', 0, '', 0, '2016-03-18 04:22:55', 'vao de', 0, 0),
(91, 4, 'test4', 0, '', 0, '2016-03-18 04:23:52', '143', 0, 0),
(92, 1, 'test1', 0, '', 0, '2016-03-22 09:13:19', 'Hello anh em', 0, 0),
(93, 3, 'test3', 0, '', 0, '2016-03-22 09:18:13', 'aaaaa', 0, 0),
(94, 4, 'test4', 0, '', 0, '2016-03-22 14:57:19', 'alo', 0, 0),
(95, 3, 'test3', 0, '', 0, '2016-03-24 12:50:33', 'alo', 0, 0),
(96, 3, 'test3', 0, '', 0, '2016-03-24 12:50:35', 'alo', 0, 0),
(97, 3, 'test3', 0, '', 0, '2016-03-24 12:50:38', 'alo', 0, 0),
(98, 3, 'test3', 0, '', 0, '2016-03-29 09:31:00', 'Ãª', 0, 0),
(99, 3, 'test3', 0, '', 0, '2016-03-31 07:43:46', 'test 3 goi test 1 nghe ro tra loi', 0, 0),
(100, 5, 'test5', 0, '', 0, '2016-03-31 09:17:09', 'ok', 0, 0),
(101, 6, 'test6', 0, '', 0, '2016-03-31 10:16:59', 'a', 0, 0),
(102, 5, 'test5', 0, '', 46, '2016-04-01 04:12:16', 'hihi', 0, 0),
(103, 5, 'test5', 0, '', 45, '2016-04-04 09:48:19', 'Alo', 0, 0),
(104, 6, 'test6', 0, '', 0, '2016-04-06 09:07:12', 'a', 0, 0),
(105, 6, 'test6', 0, '', 0, '2016-04-06 09:07:13', 'a', 0, 0),
(106, 6, 'test6', 0, '', 0, '2016-04-06 09:07:14', 'a', 0, 0),
(107, 6, 'test6', 0, '', 45, '2016-04-06 09:07:46', 'a', 0, 0),
(108, 6, 'test6', 0, '', 45, '2016-04-06 09:07:47', 'a', 0, 0),
(109, 11, 'kimhc0210', 0, '', 45, '2016-04-12 09:34:02', 'c lo', 0, 0),
(110, 10, 'tranvutuan', 0, '', 0, '2016-05-10 09:13:26', 'TÃ¬nh hÃ¬nh sao', 0, 0),
(111, 14, 'qthai2502', 0, '', 0, '2016-05-10 09:13:57', 'ko vao dc', 0, 0),
(112, 14, 'qthai2502', 0, '', 0, '2016-05-10 09:14:07', '=)))', 0, 0),
(113, 10, 'tranvutuan', 0, '', 56, '2016-05-10 09:14:31', 'KIM AN Lá»’N', 0, 0),
(114, 16, 'wantlose', 0, '', 56, '2016-05-10 09:17:39', '@@', 0, 0),
(115, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:34', 'a', 0, 0),
(116, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:35', 'a', 0, 0),
(117, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:36', 'a', 0, 0),
(118, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:36', 'a', 0, 0),
(119, 16, 'wantlose', 0, '', 0, '2016-05-23 09:24:36', 'a', 0, 0),
(120, 16, 'wantlose', 0, '', 66, '2016-05-23 09:25:06', 'ae Æ¡i', 0, 0),
(121, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:24', 'a', 0, 0),
(122, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:25', 'a', 0, 0),
(123, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:29', 'a', 0, 0),
(124, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:29', 'a', 0, 0),
(125, 16, 'wantlose', 0, '', 66, '2016-05-23 09:26:30', 'a', 0, 0),
(126, 14, 'qthai2502', 0, '', 66, '2016-05-23 09:26:32', 'vba', 0, 0),
(127, 14, 'qthai2502', 0, '', 0, '2016-06-09 09:07:36', 'h', 0, 0),
(128, 18, 'kakashilfc', 0, '', 0, '2016-06-09 09:25:00', 'ldslafdsa', 0, 0),
(129, 18, 'kakashilfc', 0, '', 83, '2016-06-09 09:25:17', 'dsfadf', 0, 0),
(130, 11, 'kimhc0210', 0, '', 83, '2016-06-09 09:48:26', 'fuck', 0, 0),
(131, 10, 'tranvutuan', 0, '', 85, '2016-06-09 10:02:58', 'Cho KÃ­m', 0, 0),
(132, 14, 'qthai2502', 0, '', 85, '2016-06-09 10:03:00', 'a', 0, 0),
(133, 14, 'qthai2502', 0, '', 85, '2016-06-09 10:03:01', 's', 0, 0),
(134, 18, 'kakashilfc', 0, '', 85, '2016-06-09 10:03:29', 'fdsafdsa', 0, 0),
(135, 19, 'angocbkhn', 0, '', 85, '2016-06-09 10:11:50', 'co ai ko', 0, 0),
(136, 11, 'kimhc0210', 0, '', 85, '2016-06-10 01:28:28', 'hello', 0, 0),
(137, 10, 'tranvutuan', 0, '', 97, '2016-06-15 09:25:15', 'KIM AN LOL', 0, 0),
(138, 19, 'angocbkhn', 0, '', 97, '2016-06-15 09:25:42', 'sao ko co ip nhi', 0, 0),
(139, 19, 'angocbkhn', 0, '', 97, '2016-06-15 09:25:51', 'alo', 0, 0),
(140, 19, 'angocbkhn', 0, '', 97, '2016-06-15 09:25:55', 'alo', 0, 0),
(141, 10, 'tranvutuan', 0, '', 97, '2016-06-17 09:41:46', 'Kim an lol', 0, 0),
(142, 10, 'tranvutuan', 0, '', 106, '2016-07-08 09:03:57', 'KIm Äƒn lÃ´l', 0, 0),
(143, 10, 'tranvutuan', 0, '', 106, '2016-07-08 09:04:16', 'Kim máº·t lol', 0, 0),
(144, 17, 'duongdt', 0, '', 0, '2016-07-08 09:08:13', 'ALo', 0, 0),
(145, 16, 'wantlose', 0, '', 0, '2016-07-13 09:31:59', 'a', 0, 0),
(146, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:03', 'dgsd', 0, 0),
(147, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:03', 'g', 0, 0),
(148, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:11', 'dg', 0, 0),
(149, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:11', 'sd', 0, 0),
(150, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:12', 'f', 0, 0),
(151, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:12', 'sd', 0, 0),
(152, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:12', 'sf', 0, 0),
(153, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:13', 's', 0, 0),
(154, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:13', 'fsd', 0, 0),
(155, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:13', 'fs', 0, 0),
(156, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:14', 'f', 0, 0),
(157, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:14', 'sf', 0, 0),
(158, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:14', 's', 0, 0),
(159, 16, 'wantlose', 0, '', 0, '2016-07-13 09:32:15', 'fs', 0, 0),
(160, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:48:20', 'alo', 0, 0),
(161, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:48:32', 'cÃ³ ai khÃ´ng', 0, 0),
(162, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:48:46', 'chÃ©m ko?', 0, 0),
(163, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:49:05', '` hello', 0, 0),
(164, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:49:07', 'hello', 0, 0),
(165, 20, 'batvuonggia', 0, '', 112, '2016-07-19 06:49:17', '13h30', 0, 0),
(166, 20, 'batvuonggia', 0, '', 0, '2016-07-19 06:53:44', 'd', 0, 0),
(167, 20, 'batvuonggia', 0, '', 112, '2016-07-22 05:31:32', 'alo', 0, 0),
(168, 16, 'wantlose', 0, '', 112, '2016-07-25 09:28:00', 'a', 0, 0),
(169, 16, 'wantlose', 0, '', 112, '2016-07-25 09:28:01', 'a', 0, 0),
(170, 16, 'wantlose', 0, '', 112, '2016-07-25 09:28:03', 'aa', 0, 0),
(171, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:25', '2', 0, 0),
(172, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:30', '1', 0, 0),
(173, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:31', '3', 0, 0),
(174, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:31', '5', 0, 0),
(175, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:37', '8', 0, 0),
(176, 16, 'wantlose', 0, '', 112, '2016-07-25 09:29:38', '-', 0, 0);

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
  `last_active` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `avatar` varchar(1000) NOT NULL,
  `level` smallint(6) NOT NULL,
  `diamond` int(11) NOT NULL,
  `status` varchar(1000) NOT NULL,
  `state` tinyint(2) NOT NULL DEFAULT '1' COMMENT '-1=off,0=invi,1=onl'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `user_caches`
--

INSERT INTO `user_caches` (`user_id`, `user_name`, `user_email`, `password`, `last_active`, `avatar`, `level`, `diamond`, `status`, `state`) VALUES
(9, 'admintest1', '', 'Admin12345678', '2016-08-04 17:37:20', '', 0, 0, '', 1),
(10, 'tranvutuan', '', 'Tuan@123', '2016-08-04 12:34:16', '', 0, 0, 'yeu doi vai luong', 1),
(11, 'kimhc0210', '', 'A123987@@', '2016-08-01 10:26:57', '', 0, 0, '', 1),
(12, 'CyberZ', '', 'Zone@$132', '2016-08-01 10:26:31', '', 0, 0, '', 1),
(13, 'dinhtan89', '', 'Dinhtan89', '2016-08-01 16:05:14', '', 0, 0, '', 1),
(14, 'qthai2502', '', 'ABC123456789', '2016-08-01 15:58:50', '', 0, 0, '', 1),
(15, 'duongdt49mt221189', '', 'D07u4ndu0n6', '2016-04-13 10:14:09', '', 0, 0, '', 1),
(16, 'wantlose', '', 'Duong168', '2016-08-01 10:26:39', '', 0, 0, '', 1),
(17, 'duongdt', '', 'D07u4ndu0n6', '2016-08-01 10:28:37', '', 0, 0, '', 1),
(18, 'kakashilfc', '', 'Kakashi123@', '2016-08-01 10:27:41', '', 0, 0, '', 1),
(19, 'angocbkhn', '', 'Ngoc12a3', '2016-08-01 10:27:14', '', 0, 0, '', 1),
(20, 'batvuonggia', '', 'Loveofromeo@@123', '2016-07-22 07:33:55', '', 0, 0, '', 1),
(21, 'nguycongtu', '', 'Vxn6bbx5', '2016-08-01 10:10:13', '', 0, 0, '', 1);

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
