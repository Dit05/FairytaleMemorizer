using System;
using System.Collections.Generic;


namespace Bemagoló {

    public abstract class QuestionAsker {

        public List<QuestionAnswerPair> qaps = new();

        public abstract Question AskQuestion(Random rand);

    }


    /// <summary>
    /// Represents a question asker that can be enabled by the user.
    /// </summary>
    public interface IConcreteAsker {

        string Title { get; }
        string Description { get; }

    }

    public interface IUserConfigurableAsker : IConcreteAsker {

        void Configure();

    }

}
