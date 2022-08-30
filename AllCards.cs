using System.Collections.Generic;
using System.IO;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using AdaptiveCards;


namespace Microsoft.BotBuilderSamples
{
    public static class Cards
    {
        public static Attachment CreateAdaptiveCardAttachment()
        {
            // combine path for cross platform support
            var paths = new[] { ".", "Resources" };
            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };

            return adaptiveCardAttachment;
        }
        public static HeroCard GetHeroCard1()
        {
            var heroCard = new HeroCard
            {
                Text = "I received a friend request from a stranger on Skype." +
                "I thought I was getting response from my post at Language Exchange." +
                " We chatted briefly then he asked for a video call. The screen was dark, though " +
                "the audio was normal. He said it was technical problems. A few minutes later, he suddenly said" +
                ", You are so beautiful.” Then I could see him on screen. He was masturbating and his lower " +
                "part was naked. I was shocked and offended, and immediately turned off Skype and blocked him. " +
                "At that very moment, I was in the library, I was terrified and worried that it might have been " +
                "seen by others."
            };

            return heroCard;
        }
        public static HeroCard GetHeroCard2()
        {
            var heroCard = new HeroCard
            {
                Text = "A man was pretending to conduct a questionnaire survey with students. He posed a number of personal and inappropriate questions, kept asking me to send him personal photos, and repeatedly made harassing phone calls."
            };
            return heroCard;
        }
        public static HeroCard GetHeroCard3()
        {
            var heroCard = new HeroCard
            {
                Text = "Sometimes friends share funny messages in chat group. Some of the messages or photos are of a sexual nature. I am not particularly offended but I think it is not necessary to make fun with sex."

            };
            return heroCard;
        }
        public static HeroCard GetHeroCard4()
        {
            var heroCard = new HeroCard
            {
                Text = "Some students always talk about sex-related topics in their conversation. They’ll ask something like, ‘Are you a virgin? Have you had sex? Did you use a condom?’ ... Sometimes if you fail a game, your punishment is that you have to answer whatever being asked and they love asking those questions which sound funny to them ... you may just laugh it off ... Some female group-mates [in orientation camp] felt such questions too pushy. Some group leaders would then just talk something else and pass over it. (Female, Undergraduate)"
            };
            return heroCard;
        }
        public static HeroCard GetHeroCard5()
        {
            var heroCard = new HeroCard
            {
                Text = "In a student gathering, several female students asked the interviewee to take a photo together with some other girls. The interviewee found that they asked her to take photos to compare their body shape and breast size. She was mocked that “these girls have boobs while this and this don’t have ...” The interviewee recalled, “I felt very embarrassed and just walked away.” (Female, Undergraduate)"

            };
            return heroCard;
        }
        public static HeroCard GetHeroCard6()
        {
            var heroCard = new HeroCard
            {
                Text = "Sometimes during a class of my major, two male students sitting in front of me liked talking nonsense. They once said that no one wanted me even I paid for it, and no one wanted me even I whored. They also called one of my friends intersex ... They always talked nonsense and found it fun.” (Female, Undergraduate)"

            };
            return heroCard;
        }
        public static HeroCard GetHeroCard7()
        {
            var heroCard = new HeroCard
            {
                Text = "“I took part in an entrepreneurship competition organised by the university. Three mentors from various fields gave us advice. I met a business incubator on behalf of my team in a coffee shop. He started talking weird, like all of sudden grabbed my hand ... I pushed his hand away, I told him I was recording and asked him to continue our discussion ... then he behaved himself” ... “While we were leaving the coffee shop, he run his hand over my hair all along to my back. I asked what he was doing. He said ‘I thought you were more open-minded’. ... After that, he often called or texted me at three o’clock in the morning to talk about the competition ...” (Female, Undergraduate, Interview #2)"

            };
            return heroCard;
        }

        public static HeroCard GetHeroCard8()
        {
            var heroCard = new HeroCard
            {
                Text = "“I still remembered what happened last year during my overseas internship … A customer praised me in front of my male co-ordinator, that is my supervisor… The male supervisor touched my back like this for seconds, saying ‘I know she is smart.’ I was very uncomfortable at that moment, felt like he was taking advantage of me. Even you may think it’s cultural difference, I did not think it was necessary to touch me or my back if you wanted to praise me. It was so intimate, like how your boyfriend hug around your waist … for approximately five seconds … I was terrified and didn’t know what to do.” (Female, Undergraduate, Interview #25)"
            };
            return heroCard;
        }
        public static HeroCard GetHeroCard9()
        {
            var heroCard = new HeroCard
            {
                Text = "“I was wearing a skirt that day. When I was having my drinks, a male student came over and put his hand on my thigh, asking if I felt cold. I didn’t know how to react. He kept his hand on my thigh until I said I did not feel cold. After that, I noticed the guy did that to other female students in class all the time, regardless he knows them well or not.” (Female, Undergraduate)"

            };
            return heroCard;
        }
        public static HeroCard GetHeroCard10()
        {
            var heroCard = new HeroCard
            {
                Text = "The interviewee had dinner with team members after training. Later she shared a taxi with a male student from another sports team to go back to the residential hall. “All of a sudden he touched my breast and grabbed it, saying, ‘Wow! Why are your boobs becoming so big!’ I pushed his hand aside right away saying, ‘What are you doing? Don’t do that ...’ After we got off the taxi, he asked me to do blow job for him, saying he feels horny and wanted my help. I told him he was insane. … After that, he had called me but I never contacted him again.” (Female, Postgraduate)"

            };
            return heroCard;
        }
        public static HeroCard GetHeroCard11()
        {
            var heroCard = new HeroCard
            {
                Text = "One night, some classmates and the interviewee drank chill in a dorm room. “A guy sat next to me but we could barely see each other in dim light ... He was gutsy to touch me, grope my leg and take my hand. He even whispered to my ear, asking if I can sleep with him that night ...” (Female, Postgraduate, Interview #9)"

            };
            return heroCard;
        }

        public static HeroCard GetHeroCard12()
        {
            var heroCard = new HeroCard
            {
                Text = "“Some orientation camps have a culture to spot the good-looking and sexy girls ... this culture is so prevalent in our university. In my second year, I was a helper at the orientation camp. The organisers kept on saying, ‘This one is pretty! This one has long legs! I want her in my group!’ Those leaders would fight for girls and say, ‘let me the male group leader take care of them and have a room for us.’ Many uncomfortable remarks were going around, which started well before orientation camp.” (Female, Undergraduate)"

            };
            return heroCard;
        }

        public static HeroCard GetHeroCard13()
        {
            var heroCard = new HeroCard
            {
                Text = "“The game ‘Once in a lifetime’ is originally designed to encourage students to think out of the box as they now study in university rather secondary school … All boys and girls played together … When I took off my eye mask ... I saw the helpers covering some students with towels ... I guessed those students had taken off their clothes. I clearly remembered the moment I opened my eyes, there were a bunch of helpers sitting in front of me, which meant they were watching us playing ... After that, I overheard a guy saying, ‘the one in group six is hot!’” (Female, Undergraduate)"

            };
            return heroCard;
        }

        public static HeroCard GetHeroCard14()
        {
            var heroCard = new HeroCard
            {
                Text = "“A group of first years all packed into the tutor’s room, not a big room, but seemed to have one hundred-ish students there, it was packed and actually everyone was squeezed shoulder to shoulder. They were blindfolded to play a game, being guided as if they need to take off all the clothes, then a few first years wanted to opt out of the game. I heard that those first years were boycotted after that, so I thought the students were forced to play the game because of peer pressure.” (Female, Postgraduate)"
            };
            return heroCard;

        }//used twice

        public static HeroCard GetHeroCard15()
        {
            var heroCard = new HeroCard
            {
                Text = "(The interviewee heard that there were first years and their seniors playing drinking games in the residential hall.) She heard that the game requires each player passing alcohol by their mouth. After drinking for a while, a second year male student took a female first year out. Some students went to a room to look for them, but the door was locked, so they stayed at the door. After a while, the girl came out in tears, asking female group leaders in another hall for help. It seemed that the guy had made out with the first year. This incident was on everyone’s lips, and became a big thing widely spreading over the residential hall. (Female, Undergraduate)"
            };
            return heroCard;
        }
        public static HeroCard GetHeroCard16()
        {
            var heroCard = new HeroCard
            {
                Text = "(Some students joined the student activities in private outside Hong Kong.) “We were quarrelling in the hotel room. He said he had spent so much money to book this hotel for me. I told him I wanted to go back to my hotel, but then he pressed me on the bed, attempted to molest me. I shouted out loud, yelling that I wanted to go back to my hotel. The soundproof is poor. The staff at front desk heard the noise, came over and knocked the door, then the guy let me go.” (Others (Gender), Undergraduate)"
            };
            return heroCard;
        }
        public static HeroCard GetHeroCard17()
        {
            var heroCard = new HeroCard
            {
                Text = "If you want to know more about others' experiences, you can go to https://www.eoc.org.hk/eoc/Upload/ResearchReport/SH2018/SHR_PM.pdf"
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard1()
        {
            var heroCard = new HeroCard
            {
                Title = "Say No",
                Text = "Make it clear to the harasser that you do not welcome the behaviour and it must stop, forexample by yelling at him/her to stop, pushing him/her away, or sending a text message to voice your objection to such behaviour."
            };
            return heroCard;
        }
        public static HeroCard GetVictimCard2()
        {
            var heroCard = new HeroCard
            {
                Title = "Jot down what happened",
                Text = "Record the details of the incident as soon as you can, including the date, time and place, the events that took place (e.g. what the harasser did, how you reacted), and whether there were other people around. These notes may come in handy when you lodge a complaint, locate witnesses or decide to take legal action."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard3()
        {
            var heroCard = new HeroCard
            {
                Title = "Jot down what happened",
                Text = "Record the details of the incident as soon as you can, including the date, time and place, the events that took place (e.g. what the harasser did, how you reacted), and whether there were other people around. These notes may come in handy when you lodge a complaint, locate witnesses or decide to take legal action. " +
                "\nWhat are the benefits ?" +
                "\r-Writing can help you make sure you don�t forget certain details." +
                "\r- Writing can be a way to process trauma." +
                "\r- It can help you share what happened later with an attorney, therapist, friend, or your school." +
                "\r- It can be a helpful starting point for a declaration or other legal statement, including a Victim Impact Statement, Restraining Orders, or a Title IX closing or opening statement." +
                "\r- It can help you report to police, if you decide to move forward with a criminal complaint." +
                "\nWhat are the potential risks ?" +
                "\r-It may get into the wrong hands - someone you don�t trust may see it." +
                "\r- You may risk weakening a legal argument down the road." +
                "\r- If you admit to alcohol or drug use in writing, it could potentially be used against you." +
                "\r- If you decided to proceed with a legal action, your writing could be subpoenaed(and ultimately viewed by the perpetrator and their attorney)." +
                "\r-It could be triggering if you are going back and forth and challenging your own versions. Remember that the point of writing down the incident is to get it out of your memory and not to question or relive it."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard4()
        {
            var heroCard = new HeroCard
            {
                Title = "Lodge a complaint",
                Text = "Make a complaint to your employer, school or the relevant organisation (e.g. a service provider) and ask that they handle it in a fair and impartial manner. If needed, seek assistance from the EOC, social workers, trade unions, NGOs, your friends or family." +
                "\nWhat are the benefits ?" +
                "\r-You may receive school accommodations like changes to your schedule, where you live, a leave of absence, or a no contact order to keep the offender away." +
                "\r- It might help prevent the same thing from happening to other students." +
                "\r- It creates a papertrail to show that you reported what happened(sometimes helpful in a future legal case)." +
                "\r- If the school decides not to take action or doesn�t follow proper procedure, you have a right to file a grievance against the school." +
                "\r- Possible accommodation will help you access your right to education and will provide you time to heal without worrying about your performance at the school." +
                "\r- It is your right to cooperate with the investigation at school, in case you decide not to pursue any actions." +
                "\nWhat are the potential risks ?" +
                "\r-Title IX has a duty to both you and the offender(if they are also a student at your school)." +
                "\rEvery school�s Title IX process is different - check with your school to learn more about what their process includes, and what standard of evidence they use." +
                "\r- Once you report, the process & outcome may be out of your control."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard5()
        {
            var heroCard = new HeroCard
            {
                Title = "Get help from the Equal Opportunity Committee",
                Text = "Contact the EOC within 12 months of the incident and lodge a complaint in writing by post or fax, or by using the online form on the EOC website."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard6()
        {
            var heroCard = new HeroCard
            {
                Title = "File a lawsuit at the District Court",
                Text = "Issue civil proceedings at the District Court within 24 months of the incident. " +
                "\n If you previously lodged a complaint with the EOC and were unable to settle the dispute through conciliation, then the period that elapsed between the date you lodged your complaint and the date the relevant conciliation failed would be disregarded in the calculation of the 24 - month limitation period."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard7()
        {
            var heroCard = new HeroCard
            {
                Title = "Make a report to the police",
                Text = "Some acts of sexual harassment may amount to criminal offences. You may report the incident to the police and lodge a complaint with the EOC concurrently. " +
                "\n What are the benefits ?" +
                "\r-Reporting to the police can provide an immediate sense of action." +
                "\r- The police can help you obtain a criminal protective order." +
                "\r- This can be useful for any legal action you take later - including a civil lawsuit, a claim for crime victims assistance funds, or even your immigration case (U Visa)." +
                "\r- If you report within 72 hours, a rape kit collects vital evidence from your body that you won�t have access to later if you don�t." +
                "\r- You could potentially help protect your community by putting the police on notice, or creating a trail of evidence of repeated allegations against an offender." +
                "\n What are the potential risks ?" +
                "\r -The officer you talk to may not be trained in responding to people in your circumstances." +
                "\r -A police officer�s role is to investigate what happened -so they may ask you questions that make you feel like you are not believed and could potentially be retraumatizing." +
                "\r- You may not trust police based on past experience." +
                "\r-You may not have control over what legal proceedings come next." +
                "\r-While preserving forensic evidence of the crime can be important to building a case against your assailant, these exams can be difficult experiences. Survivors have described forensic exams as retraumatizing and intrusive. If you can, please consider asking a friend to take you to the hospital and home afterwards so you feel more comfortable."

            };
            return heroCard;
        }

        public static HeroCard GetVictimCard8()
        {
            var heroCard = new HeroCard
            {
                Title = "Don't take action",
                Text = "What are the benefits?" +
                "\r- Limits who knows about what happened." +
                "\r- Gives you time to figure out what you want to do." +
                "\r- Allows you to process trauma in your own time." +
                "\n What are the potential risks ?" +
                "\r- You may be risking your own safety - especially if this person is still a threat to you." +
                "\r- You may be going without help from your school that you are legally entitled to -like changes to your schedule, dorm location, or a no contact order to protect you from the perpetrator and allow you time to process trauma and continue your education. If your school does not know, they can�t help you." +
                "\r- You may worry that it could happen to someone else." +
                "\r- You may not be able to access supportive services or therapy." +
                "\r-Some legal options are time sensitive, and you might run out of time depending on how long you wait." +
                "\r- You may be risking your health if you have hidden injuries or want to explore options for unwanted pregnancy or STDs."

            };
            return heroCard;
        }

        public static HeroCard GetVictimCard9()
        {
            var heroCard = new HeroCard
            {
                Title = "Find other victims",
                Text = "What are the benefits?" +
                "\r- You could coordinate a plan of action together." +
                "\r- It could be meaningful to learn you are not the only one." +
                "\r- It could be beneficial to put your campus on notice that you are not the only victim, which might lead to more reasonable measures if the campus is not taking actions to protect other students against the repeat offender." +
                "\n What are the potential risks ?" +
                "\r- You may be putting yourself at risk by sharing intimate details or by relying on someone who may have different motivations than you." +
                "\r- You have no control over what the other person will do or who they might tell." +
                "\r- It could be futile and may be more frustrating if they decide not to take action and you don't have more support for your case."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard10()
        {
            var heroCard = new HeroCard
            {
                Title = "Confront your perpetrator",
                Text = "What are the benefits?" +
                "\r- You can get out what you want to say - which potentially can provide emotional relief." +
                "\r- Depending on your relationship and the circumstances, they could be receptive to the harm they�ve caused and the need to change their ways" +
                "\n What are the potential risks ?" +
                "\r- Emotional risk - you have no idea what the person will say or what they will do with the information." +
                "\r- Safety risk - if you suffered violence or do not feel safe around this person, confrontation, even over email or text, can aggravate a dangerous situation." +
                "\r- What you say to them may be used against you if you do choose to tell your school or the police, etc." +
                "\r- This might be encouraging for some perpetrators who desire to cause harm and may enjoy hearing that they have done so.."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard11()
        {
            var heroCard = new HeroCard
            {
                Title = "Post on social media",
                Text = "What are the benefits?" +
                "\r- Social media is an accessible platform, making it an easy option." +
                "\r- It can allow you to get things off your chest." +
                "\r- It might potentially protect your community." +
                "\n What are the potential risks ?" +
                "\r- You could be sued for defamation - which could cost you a lot of money and time to defend.Suing survivors for defamation is a tool offenders use to get the survivor to recant what they have said." +
                "\r- You could ruin your own reputation if you are not believed." +
                "\r- You could face consequences at school, leading to a disruption in your education." +
                "\r-Social media posts last forever -you do not have control over who views them -even if you restrict your settings or later delete the post. Future bosses, partners, friends, teachers, may all have access to your post."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard12()
        {
            var heroCard = new HeroCard
            {
                Title = "Talk to an attorney",
                Text = "What are the benefits?" +
                "\r-Attorneys have a duty of confidentiality." +
                "\r- They can help you think through your legal options and figure out what you want to do next." +
                "\r- They can help you explore whether and how to achieve what you want: whether that is punishment for the offender, protection for yourself, immigration relief, educational benefits, financial compensation, etc." +
                "\r- Attorneys can prepare you for reporting to police or to Title IX, etc., and can help you feel less alone in the process." +
                "\r- They can provide you additional information on time limits for filing cases." +
                "\r-They can help you write a victim impact statement, and assist with filing for Victim Compensation." +
                "\r-If the attorney takes your case, they can represent you in legal proceedings."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard13()
        {
            var heroCard = new HeroCard
            {
                Title = "Tell a whisper network",
                Text = "What are the benefits?" +
                "\r-Warning your friends or community may help keep them safe." +
                "\r- By sharing the information with others, you may learn that you are not the first person harmed by your offender." +
                "\n What are the potential risks ?" +
                "\r-You don�t have control over what whispers are shared, who is believed, or how what you say is interpreted." +
                "\r- If the whispers get loud enough(i.e., published on social media), you may get sued for defamation." +
                "\r- It could impact your Title IX claim on campus." +
                "\r- It might impact the experience and support you get on campus from other friends or acquaintances." +
                "\r- You could risk your reputation."
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard14()
        {
            var heroCard = new HeroCard
            {
                Title = "send email to HKUST Gender Equity Officer",
                Text = "Email: gdc@ust.hk"
            };
            return heroCard;
        }

        public static HeroCard GetVictimCard15()
        {
            var heroCard = new HeroCard
            {
                Title = "Meeting with counsellors",
                Text = "https://counsel.hkust.edu.hk/" +
                "\r- https://counsel.hkust.edu.hk/make_an_appointment.php" +
                "\r-Psychological Assessment:" +
                "\r-https://counsel.hkust.edu.hk/page.php?section=Resources&subsection=20&anchor=selfassessment"
            };
            return heroCard;
        }


        //Adaptive Cards
        public static Attachment Victim1()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim1.json")).Card
            };
        }

        public static Attachment Victim2()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim2.json")).Card
            };
        }

        public static Attachment Victim3()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim3.json")).Card
            };
        }

        public static Attachment Victim4()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim4.json")).Card
            };
        }

        public static Attachment Victim5()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim5.json")).Card
            };
        }

        public static Attachment Victim6()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim6.json")).Card
            };
        }

        public static Attachment Victim7()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim7.json")).Card
            };
        }

        public static Attachment Victim8()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim8.json")).Card
            };
        }

        public static Attachment Victim9()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim9.json")).Card
            };
        }

        public static Attachment Victim10()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim10.json")).Card
            };
        }

        public static Attachment Victim11()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim11.json")).Card
            };
        }

        public static Attachment Victim12()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim12.json")).Card
            };
        }

        public static Attachment Victim13()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim13.json")).Card
            };
        }

        public static Attachment Victim14()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\victim14.json")).Card
            };
        }
        public static HeroCard GetByStanderAsk1Card()
        {
            var heroCard = new HeroCard
            {
                Title = "👂LISTEN!",
                Text = "Listen to what he / she has to say and, when appropriate, suggest how to lodge a complaint.Show your support, for example by accompanying the victim when he / she lodges a complaint or giving a witness statement if you saw what happened.",
            };
            return heroCard;
        }

        public static HeroCard GetByStanderAsk2Card()
        {
            var heroCard = new HeroCard
            {
                Title = "💬REASSURE him/her!",
                Text = "Reassure him / her.Sexual assault is NEVER the survivor’s fault. No one asks to be sexually assaulted by what they wear, say or do.Let the survivor know that only the perpetrator is to blame; The survivor needs to hear that fears, anxieties, guilt, and anger are normal, understandable and acceptable emotions; Remember, no one ever deserves to be abused or harassed.",
            };
            return heroCard;
        }

        public static HeroCard GetByStanderAsk3Card()
        {
            var heroCard = new HeroCard
            {
                Title = "🙌be PATIENT!",
                Text = "Be Patient. Don’t press for details – let your friend decide how much they want to share." +
                "Ask them how you can help; Survivors have to struggle with complex decisions and " +
                "feelings of powerlessness, trying to make decisions for them may only increase that sense of powerlessness. " +
                "You can be supportive by helping your friend to identify all the available options and then " +
                "help by supporting their decision - making process. The survivor can’t just “forget it” or just move on." +
                "Recovery is a long term process and each individual moves at their own pace.",
            };
            return heroCard;
        }

        public static HeroCard GetByStanderAsk4Card()
        {
            var heroCard = new HeroCard
            {
                Title = "📢SAY!",
                Text = "Things you can say: \r- It’s not your fault \r- I’m sorry this happened \r" +
                "- I believe you \r- How can I help you ? \r-I am glad you told me \r" +
                "- I’ll support your choices \r- You’re not alone",
            };
            return heroCard;
        }

        public static HeroCard GetByStanderAsk5Card()
        {
            var heroCard = new HeroCard
            {
                Title = "🧑‍🤝‍🧑BELIEVE your Friend",
                Text = "Believe Your Friend. " +
                "The most common reason people choose not to tell anyone about sexual abuse " +
                "is the fear that the listener won’t believe them.People rarely lie or exaggerate about abuse; " +
                "if someone tells you, it’s because they trust you and needs someone to talk to." +
                "People rarely make up stories of abuse. It is not necessary for you to decide if they were " +
                "\"really hurt.\" If the survivor says they were hurt, that should be enough; " +
                "Believe what your friend tells you. It may have been difficult for them to talk to you and trust you.",
            };
            return heroCard;
        }

        // An animation card that welcomes user to join the mini quiz.
        public static AnimationCard GetMiniquizAnimationCard()
        {
            var animationCard = new AnimationCard
            {
                Title = "🎮Welcome to MiniQuiz Game",
                Subtitle = "Have Fun!😸",
                Image = new ThumbnailUrl
                {
                    Url = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png",
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "https://c.tenor.com/b3oUwj1NnO8AAAAd/cat-cute-cat.gif",
                    },
                },
            };

            return animationCard;
        }


        public static HeroCard Example1()
        {
            //var imagePath = Path.Combine(Environment.CurrentDirectory, @"example_pic\example1.jpg");
            var heroCard = new HeroCard
            {
                Images = new List<CardImage>(){
                    new CardImage("https://gdc.hkust.edu.hk/img/example1.jpg")
                },
                Text = "Paul downloads some pornographic photos from the internet, he shows them in class and forwards to his classmates."
            };
            return heroCard;
        }

        public static HeroCard Example2()
        {
            //var imagePath = Path.Combine(Environment.CurrentDirectory, @"example_pic\example1.jpg");
            var heroCard = new HeroCard
            {
                Images = new List<CardImage>(){
                    new CardImage("https://gdc.hkust.edu.hk/img/example2.jpg")
                },
                Text = "In an student activity, several students are telling sexual jokes, leering at some female students and making disparaging remarks in the presence of other students. The students find it very offensive."
            };
            return heroCard;
        }

        public static HeroCard Example3()
        {
            //var imagePath = Path.Combine(Environment.CurrentDirectory, @"example_pic\example1.jpg");
            var heroCard = new HeroCard
            {
                Images = new List<CardImage>(){
                    new CardImage("https://gdc.hkust.edu.hk/img/example3.jpg")
                },
                Text = "When John’s boss is evaluating his work performance, he is told that he can get a promotion only if he goes to bed with her."
            };
            return heroCard;
        }

        public static HeroCard Example4()
        {
            //var imagePath = Path.Combine(Environment.CurrentDirectory, @"example_pic\example1.jpg");
            var heroCard = new HeroCard
            {
                Images = new List<CardImage>(){
                    new CardImage("https://gdc.hkust.edu.hk/img/example4.jpg")
                },
                Text = "A male student is very fond of Susan, he keeps asking her out by email, phone calls, and letters, despite Susan says “No” to him every time. She feels annoyed and offended."
            };
            return heroCard;
        }

        public static HeroCard Example5()
        {
            //var imagePath = Path.Combine(Environment.CurrentDirectory, @"example_pic\example1.jpg");
            var heroCard = new HeroCard
            {
                Images = new List<CardImage>(){
                    new CardImage("https://gdc.hkust.edu.hk/img/example5.jpg")
                },
                Text = "Mary is discussing her thesis with her supervisor. She is asked on a date and hinted that she may regret it if she does not comply."
            };
            return heroCard;
        }

        public static HeroCard Example6()
        {
            //var imagePath = Path.Combine(Environment.CurrentDirectory, @"example_pic\example1.jpg");
            var heroCard = new HeroCard
            {
                Images = new List<CardImage>(){
                    new CardImage("https://gdc.hkust.edu.hk/img/example6.jpg")
                },
                Text = "At the swimming pool, a male student scans lasciviously over the body of a female student in a swimsuit. She feels embarrassed and offended."
            };
            return heroCard;
        }

        public static Attachment DOCMC()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\adaptiveVictimCards\DocumentationMC.json")).Card
            };
        }
        public static Attachment DocLongText()
        {
            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(File.ReadAllText(@".\OtherAdaptiveCards\DocumentationLongText.json")).Card
            };
        }

        public static HeroCard GetDocHandleRecord()
        {
            var heroCard = new HeroCard
            {
                Text = "**I want to remain anonymous.** \n\n We will provide you with a case number and you will need to set a password " +
                "so as to access the record in future.The University will NOT know your identity and therefore will not contact " +
                "you for further case handling.You can choose to send this record to the University later if you want. \n\n" +
                "**I want to send this record to the University with my identity disclosed.** \n\n" +
                "We will verify your identity via school email. The case record with your identity will be sent to " +
                "the Gender Discrimination Committee for handling."
            };
            return heroCard;
        }
    }
}
