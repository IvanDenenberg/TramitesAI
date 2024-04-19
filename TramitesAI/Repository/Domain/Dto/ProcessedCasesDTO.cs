﻿using TramitesAI.Business.Domain.Dto;

namespace TramitesAI.Repository.Domain.Dto
{
    public class ProcessedCasesDTO
    {
        public string Id { get; private set; }
        public string MsgId { get; private set; }
        public string Channel { get; private set; }
        public string Email { get; private set; }
        public int TypeId { get; private set; }
        public RequestDTO Request { get; private set; }
        public ResponseDTO Response { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private ProcessedCasesDTO() { }

        public static ProcessedCasesDTOBuilder Builder()
        {
            return new ProcessedCasesDTOBuilder();
        }

        public class ProcessedCasesDTOBuilder
        {
            private ProcessedCasesDTO dto = new ProcessedCasesDTO();

            public ProcessedCasesDTOBuilder Id(string id)
            {
                dto.Id = id;
                return this;
            }

            public ProcessedCasesDTOBuilder MsgId(string msgId)
            {
                dto.MsgId = msgId;
                return this;
            }

            public ProcessedCasesDTOBuilder Channel(string channel)
            {
                dto.Channel = channel;
                return this;
            }

            public ProcessedCasesDTO Build()
            {
                return dto;
            }
        }
    }
}
