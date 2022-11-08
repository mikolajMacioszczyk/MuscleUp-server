package groups.workoutParticipant.participant;

import groups.common.innerCommunicators.AbstractHttpQuery;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.UUID;

import static groups.common.utils.StringUtils.SLASH;
import static groups.common.utils.StringUtils.concatenate;

@Service
public class ParticipantHttpQuery extends AbstractHttpQuery implements ParticipantQuery {

    private static final String GET_PARTICIPANT_BY_ID_PATH = "carnets/gympass/has-active/";


    @Override
    public HttpStatus checkUserId(UUID userId, UUID fitnessClubId) {

        String path = concatenate(GET_PARTICIPANT_BY_ID_PATH, userId.toString(), SLASH, fitnessClubId.toString());

        return checkIfIdExist(path);
    }
}
